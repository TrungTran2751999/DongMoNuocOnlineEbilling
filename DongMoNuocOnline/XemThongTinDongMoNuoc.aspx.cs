using System;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Util;
using EOSCRM.Web.Shared;
using EOSCRM.Dao;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System.IO.Compression;
using System.Xml;
using System.Web.Services;
using System.Web.Script.Services;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao
{
    public partial class XemThongTinDongMoNuoc : Authentication
    {
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly CMAsDao _cmasDao = new CMAsDao();
        private readonly DongMoNuocOnlineDao _dongMoNuocOnlineDao = new DongMoNuocOnlineDao();
        private string loginName;
        private int namHD;
        private int thangHD;
        private bool checkGDXN;
        private string MAPB;
        private List<InfoTB> listInfoPh = new List<InfoTB>();
        private string tenGDXN;
        private Guid GuidRandom = Guid.NewGuid();
        private string id;
        public int totalPage;
        protected void Page_Load(object sender, EventArgs e)
        {
            var idCookie = HttpContext.Current.Request.Cookies["idUuid"];
            if (idCookie != null) id = HttpContext.Current.Request.Cookies["idUuid"].Value;
            string originFile = Server.MapPath("~");
            try
            {

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));

            }
        }


        private void LoadReferences()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;

            if (loginInfo == null) return;

            totalPage = new DongMoNuocOnlineDao().totalPage;

            var nameLogin = loginInfo.Username;
            loginName = nameLogin;
            //var urName = loginInfo.NHANVIEN.HOTEN;
            //var GDXN = _dongMoNuocOnlineDao.CheckGDXN(nameLogin);
            //if (GDXN == null)
            //{
            //    containerBtnQuaHan.Visible = false;
            //    containerBtnTamNgungCapNuoc.Visible = false;
            //}
            //else
            //{
            //    MAPB = GDXN.MAPB;
            //    checkGDXN = true;
            //}

            var now = DateTime.Now.AddMonths(-1);
            
            txtKYHD.Text = now.ToString(ConfigDao.Value(DATEFORMAT.F_MMYYYY.ToString()));
            //txtNgayTBQHTN.Text = DateTime.Now.ToString(ConfigDao.Value(DATEFORMAT.F_DDMMYYYY.ToString()));
            //txtNgayTNCN.Text = DateTime.Now.ToString(ConfigDao.Value(DATEFORMAT.F_DDMMYYYY.ToString()));
            //txtNgayNhapCS.Text = DateTime.Now.ToString(ConfigDao.Value(DATEFORMAT.F_DDMMYYYY.ToString()));

            var txtKYHDText = "01/" + txtKYHD.Text.Trim();
            namHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Year;
            thangHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Month;

            ddlTT.Items.Clear();
            ddlTT.Items.Add(new ListItem("", ""));
            ddlTT.Items.Add(new ListItem("TB thanh toán", _dongMoNuocOnlineDao.TBTN));
            ddlTT.Items.Add(new ListItem("TB nhắc nợ", _dongMoNuocOnlineDao.TBNN_1));
            ddlTT.Items.Add(new ListItem("TB quá hạn tiền nước lần 1", _dongMoNuocOnlineDao.TBQH_1));
            ddlTT.Items.Add(new ListItem("TB quá hạn tiền nước lần 2", _dongMoNuocOnlineDao.TBQH_2));
            ddlTT.Items.Add(new ListItem("TB tạm ngừng cấp nước", _dongMoNuocOnlineDao.TBTNCN));

            ddlZaloAndApp.Items.Clear();
            ddlZaloAndApp.Items.Add(new ListItem("", ""));
            ddlZaloAndApp.Items.Add(new ListItem("Đã cài zalo/app", "true"));
            ddlZaloAndApp.Items.Add(new ListItem("Chưa cài zalo/app", "false"));

            ddlIsGuiAppCSKH.Items.Clear();
            ddlIsGuiAppCSKH.Items.Add(new ListItem("", ""));
            ddlIsGuiAppCSKH.Items.Add(new ListItem("Đã gửi", "true"));
            ddlIsGuiAppCSKH.Items.Add(new ListItem("Chưa gửi", "false"));

            ddlIsGuiZalo.Items.Clear();
            ddlIsGuiZalo.Items.Add(new ListItem("", ""));
            ddlIsGuiZalo.Items.Add(new ListItem("Đã gửi", "true"));
            ddlIsGuiZalo.Items.Add(new ListItem("Chưa gửi", "false"));

            //foreach (var cmas in cmasList)
                //ddlCMAs.Items.Add(new ListItem(cmas.TENCMAs, cmas.MACMAs));

            // bind khu vuc quan ly repeater
            var kvqlList = _kvDao.GetList();
            dlKHUVUC.DataSource = kvqlList;
            dlKHUVUC.DataBind();

            var listXN = _dongMoNuocOnlineDao.GetListXNCN();
            listXN.Add("");
            listXN.Reverse();
            ddlXNCN.Items.Clear();
            ddlXNCN.DataSource = listXN;
            ddlXNCN.DataBind();

            //Lay thong tin lo trinh
            var listLoTrinh = _dongMoNuocOnlineDao.GetAllLoTrinh();
            listLoTrinh.Insert(0, new LoTrinh() { MaDP = "", TenDP=""});
            ddlLoTrinh.Items.Clear();
            foreach (var loTrinh in listLoTrinh) {
                ddlLoTrinh.Items.Add(new ListItem(loTrinh.TenDP + "-" +loTrinh.MaDP, loTrinh.MaDP));
            }


            udpIsZaloAndApp.Visible = false;
            
            //udpNgayTNCN.Visible = false;

            //UptPanel.Visible = false;
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            if (txtKYHD.Text.Trim()==null || txtKYHD.Text.Trim()=="")
            {
                CloseWaitingDialog();
                ShowError("Vui lòng chọn kỳ hóa đơn.");
                return;
            }
            var txtKYHDText = "01/" + txtKYHD.Text.Trim();
            var idkh = txtIDKH.Text.Trim();
            idkh = idkh != "" && idkh != null ? idkh : null;
            namHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Year;
            thangHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Month;
            var tenDuongPho = txtDuongPho.Text.Trim() != null && txtDuongPho.Text.Trim() != "" ? "%"+txtDuongPho.Text.Trim()+"%" : null;
            //var tenKv = txtKhuVuc.Text.Trim() != null && txtKhuVuc.Text.Trim() != "" ? txtKhuVuc.Text.Trim() : null;
            //var denky = ConvertUtil.ToDateTime("01/" + txtKYHDCuoi.Text.Trim());

            //var tungay = ConvertUtil.ToDateTime(txtTuNgay.Text.Trim());
            //var denngay = ConvertUtil.ToDateTime(txtDenNgay.Text.Trim());

            var kvList = "";
            //var tenchinhanh = "";

            for (var i = 0; i < dlKHUVUC.Items.Count; i++)
            {
                var item = dlKHUVUC.Items[i];
                var cb = item.FindControl("chkKVNV") as HtmlInputCheckBox;
                if (cb == null) continue;

                if (cb.Attributes["disabled"] == null && cb.Checked)
                {
                    var kv = _kvDao.Get(cb.Attributes["title"]);
                    //tenchinhanh += (kv != null ? kv.TENKV : "") + ", ";
                    kvList += cb.Attributes["title"] + ",";
                }
            }

            if (kvList.Length > 0)
            {
                kvList = kvList.Substring(0, kvList.Length - 1);
                //tenchinhanh = tenchinhanh.Substring(0, tenchinhanh.Length - 2);
            }


            if (kvList.Length == 0)
            {
                var kvqlList = _kvDao.GetList();
                foreach (var khuvuc in kvqlList)
                    kvList += khuvuc.MAKV + ",";

                kvList = kvList.Substring(0, kvList.Length - 1);
                //tenchinhanh = "Tất cả";
            }
            var TTSlected = ddlTT.SelectedValue != "" ? ddlTT.SelectedValue : null;
            
            var xncnSelected = ddlXNCN.SelectedValue != "" ? ddlXNCN.SelectedValue : null;

            if (txtNgayLoc.Text != "" && txtNgayLoc.Text != null)
            {
               var dateNgayLoc =  (DateTime)ConvertUtil.ToDateTime(txtNgayLoc.Text);
               if (DateTime.Compare(dateNgayLoc, DateTime.Now) > 0)
               {
                   CloseWaitingDialog();
                   ShowWarning("Ngày lọc không được muộn hơn ngày hiện tại");
                   contNhacNo.Visible = false;
                   UptPanel.Update();
                   return;
               }

            }
            var ngayLoc = txtNgayLoc.Text != "" && txtNgayLoc.Text != null ? ((DateTime)ConvertUtil.ToDateTime(txtNgayLoc.Text)).ToString("yyyy-MM-dd") : null;
            
            var maLoTrinh = ddlLoTrinh.SelectedValue != "" ? ddlLoTrinh.SelectedValue : null;

            var isZaloAndApp = ddlZaloAndApp.SelectedValue != "" ? ddlZaloAndApp.SelectedValue : null;

            var IsGuiAppCSKH = ddlIsGuiAppCSKH.SelectedValue != "" ? ddlIsGuiAppCSKH.SelectedValue : null;

            var IsGuiZalo = ddlIsGuiZalo.SelectedValue != "" ? ddlIsGuiZalo.SelectedValue : null;

            //var ngayTBQH = txtNgayTBQHTN.Text != "" && txtNgayTBQHTN .Text!=null? ConvertUtil.ToDateTime(txtNgayTBQHTN.Text) : null;
            DieuKienLoc dieuKienLoc = new DieuKienLoc
            {
                KyHd = thangHD,
                NamHd = namHD,
                Idkh = idkh,
                KhuVuc = kvList,
                XNCN = xncnSelected,
                MaDuongPho = tenDuongPho,
                NgayLoc = ngayLoc,
                MaLoTrinh = maLoTrinh,
                isZaloAndApp = isZaloAndApp,
                IsGuiAppCSKH = IsGuiAppCSKH,
                IsGuiZalo = IsGuiZalo
                //IsGuiZalo = CheckboxIsGuiZalo.Checked
            };
            //Loc khach hang de thong bao nhan no lan 1
            //var myds = new DataSet();
            var listInfo = new List<KhConNo>();
            if (TTSlected == _dongMoNuocOnlineDao.TBTN)
            {
                listInfo = _dongMoNuocOnlineDao.GetKH_TB_TienNuoc(dieuKienLoc);
                containerXuatFilePdf.Visible = false;
            }else if (TTSlected == _dongMoNuocOnlineDao.TBNN_1)
            {
                listInfo = _dongMoNuocOnlineDao.GetKH_TB_NhacNo(dieuKienLoc);
                containerXuatFilePdf.Visible = false;
            }
            //Loc khach hang de thong bao qua han lan 1
            else if (TTSlected == _dongMoNuocOnlineDao.TBQH_1)
            {
                listInfo = _dongMoNuocOnlineDao.GetKH_TB_QuaHan_1(dieuKienLoc);
                containerXuatFilePdf.Visible = false;
            }
            //Loc khach hang de thong bao qua han lan 2
            else if (TTSlected == _dongMoNuocOnlineDao.TBQH_2)
            {
                listInfo = _dongMoNuocOnlineDao.GetKH_TB_QuaHan_2_DaPheDuyet(dieuKienLoc);
                containerXuatFilePdf.Visible = true;
            }
            //Loc khach hang de thong bao tam ngung cap nuoc
            else if (TTSlected == _dongMoNuocOnlineDao.TBTNCN)
            {
                listInfo = _dongMoNuocOnlineDao.GetKH_TNCN_DaPheDuyet(dieuKienLoc);
                containerXuatFilePdf.Visible = true;
            }
            gvListKH.DataSource = listInfo;
            gvListKH.PagerInforText = listInfo.Count.ToString();
            gvListKH.DataBind();
            
            contNhacNo.Visible = true;
            UptPanel.Update();

            CloseWaitingDialog();
        }
        protected void btnDuyetTBQuaHan_Click(object sender, EventArgs e)
        {

            
        }
        protected void btnDuyetTNCapNuoc_Click(object sender, EventArgs e)
        {
            var txtKYHDText = "01/" + txtKYHD.Text.Trim();
            namHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Year;
            thangHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Month;

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;

            if (loginInfo == null) return;

            var nameLogin = loginInfo.Username;
            loginName = nameLogin;
            var GDXN = _dongMoNuocOnlineDao.CheckGDXN(loginName);
            var result = _dongMoNuocOnlineDao.PheDuyetTamNgungCapNuoc(checkGDXN, namHD, thangHD, GDXN.MAPB);
            if (result > 0)
            {
                CloseWaitingDialog();
                ShowInfor(String.Format("Duyệt {0} khách hàng tạm ngừng cấp nước thành công", result));
            }
            else if (result == -1)
            {
                CloseWaitingDialog();
                ShowWarning("Không có khách hàng nào được duyệt cắt nước");
            }
            else if (result == -2)
            {
                CloseWaitingDialog();
                ShowWarning("Bạn không đủ thực hiện quyền này");
            }
        }
        protected void btnXuatTBQHTN_Click(object sender, EventArgs e)
        {
            CloseWaitingDialog();
            //var listTBQHTN = JsonConvert.SerializeObject(listInfoPh);
            //HttpContext.Current.Session["dataTBQHTN" + id] = listTBQHTN;
            //HttpCookie cookieIsDowload = new HttpCookie("isDowload", "true");
            //Response.Cookies.Add(cookieIsDowload);
            //ReDirect("DongMoNuocOnline.aspx?id=" + GuidRandom + GuidRandom);
        }
        protected void ddlTT_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlTT.SelectedValue == "TBQH_2")
            //{
            //    udpIsZaloAndApp.Visible = true;
            //}
            //else
            //{
            //    udpIsZaloAndApp.Visible = false;
            //}

            //if (ddlTT.SelectedValue == "TBQHTN")
            //{
            //    udpNgayTBQHTN.Visible = true;
            //    udpNgayTNCN.Visible = false;
            //}
            //else if (ddlTT.SelectedValue == "TNCN")
            //{
            //    udpNgayTBQHTN.Visible = false;
            //    udpNgayTNCN.Visible = true;
            //}
            //else
            //{
            //    udpNgayTBQHTN.Visible = false;
            //    udpNgayTNCN.Visible = false;
            //}
        }
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static string GetDanhSachNhacNo(DieuKienLoc dieuKienLoc)
        //{
            //var result = new DongMoNuocOnlineDao().GetBase64PDFDuyetTBQHTN(ky, nam, tenDuongPho, kvList, idkh, xncn, ngayTBQH);
            //if (result == null) return "0";
            //return result;
        //}
        #region thong bao nhac no
            #region lay thong tin khach hang nhac no
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<KhConNo> GetListTbNhacNo(string dieuKienLocStr)
        {
            var dieuKienLoc = JsonConvert.DeserializeObject<DieuKienLoc>(dieuKienLocStr);
            if (dieuKienLoc.NgayLoc != null) dieuKienLoc.NgayLoc = ((DateTime)ConvertUtil.ToDateTime(dieuKienLoc.NgayLoc)).ToString("yyyy-MM-dd");
            var result = new DongMoNuocOnlineDao().GetKH_TB_NhacNo(dieuKienLoc);
            if (result == null) return new List<KhConNo>();
            return result;
        }
        #endregion
        #endregion

        #region thong bao qua han lan 1
            #region lay thong tin khach hang thong bao quan han tien nuoc lan 1
            [WebMethod]
            [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
            public static List<KhConNo> GetListTBQHLan1(string dieuKienLocStr)
            {
                var dieuKienLoc = JsonConvert.DeserializeObject<DieuKienLoc>(dieuKienLocStr);
                if (dieuKienLoc.NgayLoc != null) dieuKienLoc.NgayLoc = ((DateTime)ConvertUtil.ToDateTime(dieuKienLoc.NgayLoc)).ToString("yyyy-MM-dd");
                var result = new DongMoNuocOnlineDao().GetKH_TB_QuaHan_1(dieuKienLoc);
                if (result == null) return new List<KhConNo>();
                return result;
            }
            #endregion
        #endregion

        #region thong bao qua han lan 2
            #region lay thong tin khach hang thong bao quan han tien nuoc lan 2
            [WebMethod]
            [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
            public static List<KhConNo> GetListTBQHLan2DaPheDuyet(string dieuKienLocStr)
            {
                var dieuKienLoc = JsonConvert.DeserializeObject<DieuKienLoc>(dieuKienLocStr);
                if (dieuKienLoc.NgayLoc != null) dieuKienLoc.NgayLoc = ((DateTime)ConvertUtil.ToDateTime(dieuKienLoc.NgayLoc)).ToString("yyyy-MM-dd");
                var result = new DongMoNuocOnlineDao().GetKH_TB_QuaHan_2_DaPheDuyet(dieuKienLoc);
                if (result == null) return new List<KhConNo>();
                return result;
            }
            #endregion

            #region Xuat giay thong bao qua han lan 2
            [WebMethod]
            [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
            public static string GetGiayThongBaoQuaHan2(string listKHConNo, string dieuKienLocStr)
            {
                var khConNos = JsonConvert.DeserializeObject<List<KhConNo>>(listKHConNo);
                var dieuKienLoc = JsonConvert.DeserializeObject<DieuKienLoc>(dieuKienLocStr);
                if (dieuKienLoc != null) dieuKienLoc.NgayLoc = ((DateTime)ConvertUtil.ToDateTime(dieuKienLoc.NgayLoc)).ToString("yyyy-MM-dd");
                //var listKHConNo = new DongMoNuocOnlineDao().GetKH_TB_QuaHan_2_DaPheDuyet(dieuKienLoc);
                var result = new DongMoNuocOnlineDao().GetAllGiayThongBaoQuaHan2_PDF(khConNos, dieuKienLoc);
                //if (result == null) return new List<KhConNo>();
                return result;
            }

            #endregion

            #region Lay so luong thong bao qua han lan 2
            [WebMethod]
            [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
            public static KhConNo CountGiayThongBaoQuaHan2(string dieuKienLocStr)
            {
                var dieuKienLoc = JsonConvert.DeserializeObject<DieuKienLoc>(dieuKienLocStr);
                if (dieuKienLoc != null) dieuKienLoc.NgayLoc = ((DateTime)ConvertUtil.ToDateTime(dieuKienLoc.NgayLoc)).ToString("yyyy-MM-dd");
                //var listKHConNo = new DongMoNuocOnlineDao().GetKH_TB_QuaHan_2_DaPheDuyet(dieuKienLoc);
                var result = new DongMoNuocOnlineDao().Count_TB_QuaHan_DaPheDuyet(dieuKienLoc);
                //if (result == null) return new List<KhConNo>();
                return result;
            }
            #endregion
        #endregion

       #region thong bao tam ngung cap nuoc
            #region lay thong tin khach hang thong bao tam ngung cap nuoc
            [WebMethod]
            [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
            public static List<KhConNo> GetListTBTNCN(string dieuKienLocStr)
            {
                var dieuKienLoc = JsonConvert.DeserializeObject<DieuKienLoc>(dieuKienLocStr);
                if (dieuKienLoc.NgayLoc != null) dieuKienLoc.NgayLoc = ((DateTime)ConvertUtil.ToDateTime(dieuKienLoc.NgayLoc)).ToString("yyyy-MM-dd");
                var result = new DongMoNuocOnlineDao().GetKH_TNCN_DaPheDuyet(dieuKienLoc);
                if (result == null) return new List<KhConNo>();
                return result;
            }
            #endregion

            #region Xuat giay thong bao tam ngung cap nuoc
            [WebMethod]
            [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
            public static string GetGiayThongBaoTamNgungCapNuoc(string listKHConNo, string dieuKienLocStr)
            {
                var khConNos = JsonConvert.DeserializeObject<List<KhConNo>>(listKHConNo);
                var dieuKienLoc = JsonConvert.DeserializeObject<DieuKienLoc>(dieuKienLocStr);
                if (dieuKienLoc != null) dieuKienLoc.NgayLoc = ((DateTime)ConvertUtil.ToDateTime(dieuKienLoc.NgayLoc)).ToString("yyyy-MM-dd");
                //if (listKHConNo != null) dieuKienLoc.NgayLoc = ((DateTime)ConvertUtil.ToDateTime(dieuKienLoc.NgayLoc)).ToString("yyyy-MM-dd");
                //var listKHConNo = new DongMoNuocOnlineDao().GetKH_TB_QuaHan_2_DaPheDuyet(dieuKienLoc);
                var result = new DongMoNuocOnlineDao().GetAllGiayThongBaoTamNgungCapNuoc_PDF(khConNos, dieuKienLoc);
                //if (result == null) return new List<KhConNo>();
                return result;
            }
            #endregion

            #region Lay so luong thong bao tam ngung cap nuoc
            [WebMethod]
            [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
            public static KhConNo CountGiayThongBaoTamNgungCapNuoc(string dieuKienLocStr)
            {
                var dieuKienLoc = JsonConvert.DeserializeObject<DieuKienLoc>(dieuKienLocStr);
                if (dieuKienLoc != null) dieuKienLoc.NgayLoc = ((DateTime)ConvertUtil.ToDateTime(dieuKienLoc.NgayLoc)).ToString("yyyy-MM-dd");
                //var listKHConNo = new DongMoNuocOnlineDao().GetKH_TB_QuaHan_2_DaPheDuyet(dieuKienLoc);
                var result = new DongMoNuocOnlineDao().Count_TB_TNCN_DaPheDuyet(dieuKienLoc);
                //if (result == null) return new List<KhConNo>();
                return result;
            }
            #endregion
       #endregion




        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static string GetDanhSachTBQTN(int ky, int nam, string tenDuongPho, string kvList, string idkh, string xncn, string ngayTBQH)
        //{
        //    //var result = new DongMoNuocOnlineDao().GetBase64PDFDuyetTBQHTN(ky, nam, tenDuongPho, kvList, idkh, xncn, ngayTBQH);
        //    //if (result == null) return "0";
        //    //return result;
        //}
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetDanhSachTNCN(string xncn, int ky, int nam)
        {
            var result = new DongMoNuocOnlineDao().GetListKhachHangTNCN(ky, nam, xncn);
            if (result == null) return "0";
            return result;
        }
        
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        
        }
        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvListKH.PageIndex = e.NewPageIndex;
                // Bind data for grid
                //BindDataForDuyetTNCN();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #region Lộ trình
        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            //BindDuongPho();
            //CloseWaitingDialog();
        }

        private void BindDuongPho()
        {
            //var list = _dpDao.GetList("%", txtFilterDP.Text.Trim());
            //gvDuongPho.DataSource = list;
            //gvDuongPho.PagerInforText = list.Count.ToString();
            //gvDuongPho.DataBind();
        }

        private void UpdateKhuVuc(DUONGPHO dp)
        {
            //if (dp == null)
            //{
            //    SetLabel(lblTENDUONG.ClientID, "");
            //    lblTENDUONG.Text = "";
            //    return;
            //}
            //SetLabel(lblTENDUONG.ClientID, dp.TENDP);
            //lblTENDUONG.Text = dp.TENDP;

        }

        protected void btnBrowseDP_Click(object sender, EventArgs e)
        {
            //BindDuongPho();
            //upnlDuongPho.Update();
            //UnblockDialog("divDuongPho");
        }

        protected void gvDuongPho_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //try
            //{
            //    var id = e.CommandArgument.ToString();

            //    switch (e.CommandName)
            //    {
            //        case "SelectMADP":
            //            var ids = id.Split('-');
            //            if (!string.Empty.Equals(id) && ids.Length == 2)
            //            {
            //                var dp = _dpDao.Get(ids[0], ids[1]);
            //                if (dp != null)
            //                {
            //                    SetControlValue(txtMADP.ClientID, dp.MADP);
            //                    UpdateKhuVuc(dp);
            //                    upnlInfor.Update();

            //                    HideDialog("divDuongPho");
            //                    CloseWaitingDialog();

            //                    txtMADP.Focus();
            //                }
            //            }
            //            break;

            //    }
            //}
            //catch (Exception ex)
            //{
            //    DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            //}
        }
        #endregion

    }
} 
