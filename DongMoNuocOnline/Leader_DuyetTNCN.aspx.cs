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
using AjaxPro;
using System.Configuration;
using System.Web.Configuration;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao
{

    public partial class LeaderDuyetTNCN : Authentication
    {
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly CMAsDao _cmasDao = new CMAsDao();
        private readonly DongMoNuocOnlineDao _dongMoNuocOnlineDao = new DongMoNuocOnlineDao();
        public string loginName;
        private int namHD;
        private int thangHD;
        private bool checkGDXN;
        private string MAPB;
        private List<InfoTB> listInfoPh = new List<InfoTB>();
        private string tenGDXN;
        private Guid GuidRandom = Guid.NewGuid();
        private string idTBQHTN;
        private string idTNCN;
        private HopDongDao _hopDongDao = new HopDongDao();
        private List<TBQHTN> ListTBQHTN;
        private List<TNCN> ListTNCN;
        protected void Page_Load(object sender, EventArgs e)
        {
            var idCookieTBQHTN = HttpContext.Current.Request.Cookies["idUuid"];
            
            if (idCookieTBQHTN != null) idTBQHTN = HttpContext.Current.Request.Cookies["idUuid"].Value;


            //string paramId = ddlTT.SelectedValue;
            //if (paramId == "TNQHTN")
            //{
            //    containerTBQHTN.Attributes["style"] = "display:block";
            //    containerTNCN.Attributes["style"] = "display:none";
            //    upnlTBQHTN.Visible = true;
            //}
            //else if (paramId == "TNCN")
            //{
            //    containerTBQHTN.Attributes["style"] = "display:block";
            //    containerTNCN.Attributes["style"] = "display:none";
            //    upnlTBQHTN.Visible = false;
            //}
            string originFile = Server.MapPath("~");
            try
            {

                if (!Page.IsPostBack)
                {
                    
                    LoadReferences();
                    
                    //HttpCookie idServerPath = new HttpCookie("idServerPath", originFile);
                    //Response.Cookies.Add(idServerPath);
                    ////BindDataForDuyetTNCN("");
                    //var typeDownload = Request.QueryString["download"];
                    //if (HttpContext.Current.Session["dataTBQHTN" + idTBQHTN] != null && typeDownload=="TBQHTN")
                    //{
                    //    idCookieTBQHTN = HttpContext.Current.Request.Cookies["idUuid"];
                    //    if (idCookieTBQHTN != null)
                    //    {
                    //        idTBQHTN = idCookieTBQHTN.Value;
                    //    }
                    //    else
                    //    {
                    //        return;
                    //    }
                    //    string dataTBQHTN = HttpContext.Current.Session["dataTBQHTN" + idTBQHTN].ToString();
                    //    HttpContext.Current.Session["dataTBQHTN" + idTBQHTN] = null;

                    //    var listInfoTBQH = JsonConvert.DeserializeObject<List<InfoTB>>(dataTBQHTN);

                    //    string originTBQHTN = originFile + @"BienBan\ThongBaoQuaHanTienNuoc.docx";
                    //    byte[] fileBytes = _dongMoNuocOnlineDao.TaoThongBaoQuaHanTienNuocNotZipNotCreate(originTBQHTN, listInfoTBQH, originFile);

                    //    Response.Clear();
                    //    //Response.Cookies.Remove("isDowload");
                    //    //Response.Cookies.Add(isDowload);
                    //    Response.ContentType = "application/octet-stream";
                    //    Response.AddHeader("Content-Disposition", "attachment; filename=TBQHTN.docx");
                    //    Response.BinaryWrite(fileBytes);
                    //    Response.Flush();
                    //    Response.End();
                    //}
                    //String confString = HttpContext.Current.Request.ApplicationPath.ToString();
                    //Configuration conf = WebConfigurationManager.OpenWebConfiguration(confString);
                    //ScriptingJsonSerializationSection section = (ScriptingJsonSerializationSection)conf.GetSection("system.web.extensions/scripting/webServices/jsonSerialization");
                    //section.MaxJsonLength = 6553600;
                    //conf.Save();
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

            var txtKYHDText = "01/" + txtKYHD.Text.Trim();
            namHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Year;
            thangHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Month;

            //ddlTT.Items.Clear();
            //ddlTT.Items.Add(new ListItem("", ""));
            //ddlTT.Items.Add(new ListItem("TB quá hạn tiền nước", "TBQHTN"));
            //ddlTT.Items.Add(new ListItem("Tạm ngừng cấp nước", "TNCN"));
            //ddlTT.Items.Add(new ListItem("Duyệt tạm ngừng cấp nước", "D_TNCN"));
            var xncn = _dongMoNuocOnlineDao.CheckNhanVienPheDuyetTBQH(loginName);
            ddlXNCN.Items.Clear();
            if (xncn != null)
            {
                ddlXNCN.Items.Add(new ListItem(xncn.MAPB, xncn.MAPB));
            }
            //foreach (var cmas in cmasList)
                //ddlCMAs.Items.Add(new ListItem(cmas.TENCMAs, cmas.MACMAs));

            // bind khu vuc quan ly repeater
            var kvqlList = _kvDao.GetList();
            dlKHUVUC.DataSource = kvqlList;
            dlKHUVUC.DataBind();

            //var listXN = _dongMoNuocOnlineDao.GetListXNCN();
            //listXN.Add("");
            //listXN.Reverse();
            //ddlXNCN.Items.Clear();
            //ddlXNCN.DataSource = listXN;
            //ddlXNCN.DataBind();

            //Lay thong tin lo trinh
            var listLoTrinh = _dongMoNuocOnlineDao.GetAllLoTrinh();
            listLoTrinh.Insert(0, new LoTrinh() { MaDP = "", TenDP = "" });
            ddlLoTrinh.Items.Clear();
            foreach (var loTrinh in listLoTrinh)
            {
                ddlLoTrinh.Items.Add(new ListItem(loTrinh.TenDP + "-" + loTrinh.MaDP, loTrinh.MaDP));
            }

            //udpNgayTBQHTN.Visible = false;
            //udpNgayTNCN.Visible = false;
            //upnlTBQHTN.Visible = false;
            //upnlDuyetTNCN.Visible = false;
            //divDuyetTNCN.Visible = false;
            //divCR.Visible = false;
        }
        private void getValueInput()
        {

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
            var nam = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Year;
            namHD = nam;
            var thang = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Month;
            thangHD = thang;
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
            //var TTSlected = ddlTT.SelectedValue != "" ? ddlTT.SelectedValue : null;
            
            var xncnSelected = ddlXNCN.SelectedValue != "" ? ddlXNCN.SelectedValue : null;
            var ngayLoc = txtNgayLoc.Text != "" && txtNgayLoc.Text != null ? ((DateTime)ConvertUtil.ToDateTime(txtNgayLoc.Text)).ToString("yyyy-MM-dd") : null;

            var maLoTrinh = ddlLoTrinh.SelectedValue != "" ? ddlLoTrinh.SelectedValue : null;
            if (txtNgayLoc.Text != "" && txtNgayLoc.Text != null)
            {
                var dateNgayLoc = (DateTime)ConvertUtil.ToDateTime(txtNgayLoc.Text);
                if (DateTime.Compare(dateNgayLoc, DateTime.Now) > 0)
                {
                    CloseWaitingDialog();
                    ShowWarning("Ngày lọc không được muộn hơn ngày hiện tại");
                    contNhacNo.Visible = false;
                    UptPanel.Update();
                    return;
                }

            }
            DieuKienLoc dieuKienLoc = new DieuKienLoc
            {
                KyHd = thangHD,
                NamHd = namHD,
                Idkh = idkh,
                KhuVuc = kvList,
                XNCN = xncnSelected,
                MaDuongPho = tenDuongPho,
                NgayLoc = ngayLoc,
                MaLoTrinh = maLoTrinh
            };

            var listInfo = _dongMoNuocOnlineDao.GetKH_TNCN_To_Leader_PheDuyet(dieuKienLoc);

            gvListKH.DataSource = listInfo;
            gvListKH.PagerInforText = listInfo.Count.ToString();
            gvListKH.DataBind();
            UptPanel.Update();
            contNhacNo.Visible = true;

            
            CloseWaitingDialog();
        }
        protected void btnDuyetTBQuaHan_Click(object sender, EventArgs e)
        {

            //var txtKYHDText = "01/" + txtKYHD.Text.Trim();
            //namHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Year;
            //thangHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Month;

            //var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;

            //if (loginInfo == null) return;

            //var nameLogin = loginInfo.Username;
            //loginName = nameLogin;
            //var GDXN = _dongMoNuocOnlineDao.CheckGDXN(loginName);
            //var result = _dongMoNuocOnlineDao.PheDuyetQuaHanTienNuoc(checkGDXN, namHD, thangHD, GDXN.MAPB);
            //CloseWaitingDialog();
            //if (result > 0)
            //{
            //    ShowInfor(String.Format("Duyệt {0} khách hàng quá hạn thanh toán thành công", result));
            //    //ShowInfor("khách hàng quá hạn thanh toán thành công");
            //}
            //else if (result == -1)
            //{
            //    ShowWarning("Không có khách hàng nào quá hạn tìền nước");
            //}
            //else if (result == -2)
            //{
            //    ShowWarning("Bạn không đủ thực hiện quyền này");
            //}
        }
        protected void btnDuyetTNCapNuoc_Click(object sender, EventArgs e)
        {
            //var txtKYHDText = "01/" + txtKYHD.Text.Trim();
            //namHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Year;
            //thangHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Month;

            //var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;

            //if (loginInfo == null) return;

            //var nameLogin = loginInfo.Username;
            //loginName = nameLogin;
            //var GDXN = _dongMoNuocOnlineDao.CheckGDXN(loginName);
            //var result = _dongMoNuocOnlineDao.PheDuyetTamNgungCapNuoc(checkGDXN, namHD, thangHD, GDXN.MAPB);
            //if (result > 0)
            //{
            //    CloseWaitingDialog();
            //    ShowInfor(String.Format("Duyệt {0} khách hàng tạm ngừng cấp nước thành công", result));
            //}
            //else if (result == -1)
            //{
            //    CloseWaitingDialog();
            //    ShowWarning("Không có khách hàng nào được duyệt cắt nước");
            //}
            //else if (result == -2)
            //{
            //    CloseWaitingDialog();
            //    ShowWarning("Bạn không đủ thực hiện quyền này");
            //}
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
        //protected void ddlTT_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //Console.WriteLine(ddlTT.SelectedValue);
            
        //    if (ddlTT.SelectedValue == "TBQHTN")
        //    {
        //        udpNgayTBQHTN.Visible = true;
        //        udpNgayTNCN.Visible = false;
        //    }
        //    else if (ddlTT.SelectedValue == "TNCN")
        //    {
        //        udpNgayTBQHTN.Visible = false;
        //        udpNgayTNCN.Visible = true;
        //    }
        //    else if (ddlTT.SelectedValue == "D_TNCN")
        //    {
        //        var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;

        //        if (loginInfo == null) return;

        //        var nameLogin = loginInfo.Username;
        //        loginName = nameLogin;

        //        GDXN checkGDXN = null;
        //        if (loginName != null)
        //        {
        //            checkGDXN = _dongMoNuocOnlineDao.CheckGDXN(loginName);
        //            if (checkGDXN.MAPB == "CNH")
        //            {
        //                ddlXNCN.SelectedValue = "Huế";
        //            }
        //            else if (checkGDXN.MAPB == "CNHD")
        //            {
        //                ddlXNCN.SelectedValue = "Hương Điền";
        //            }
        //            else if (checkGDXN.MAPB == "CNHP")
        //            {
        //                ddlXNCN.SelectedValue = "Hương Phú";
        //            }
        //            ddlXNCN.Enabled = false;
        //        }
        //    }
        //    if (ddlTT.SelectedValue != "D_TNCN") ddlXNCN.Enabled = true;
        //    if (ddlTT.SelectedValue != "TNCN" && ddlTT.SelectedValue != "TBQHTN")
        //    {
        //        udpNgayTBQHTN.Visible = false;
        //        udpNgayTNCN.Visible = false;
        //    }
        //}
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static int PheDuyetTNCN(string dieuKienLocStr, string loginName)
        {
            var dieuKienLocObj = JsonConvert.DeserializeObject<DieuKienLoc>(dieuKienLocStr);
            if (dieuKienLocObj.NgayLoc != null) dieuKienLocObj.NgayLoc = ((DateTime)ConvertUtil.ToDateTime(dieuKienLocObj.NgayLoc)).ToString("yyyy-MM-dd");
            var resp = new DongMoNuocOnlineDao().LeaderPheDuyetThongBaoTamNgungCapNuoc(dieuKienLocObj, loginName);
            return resp;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static int HuyPheDuyetTNCN(string dieuKienLocStr)
        {
            var dieuKienLocObj = JsonConvert.DeserializeObject<DieuKienLoc>(dieuKienLocStr);
            if (dieuKienLocObj.NgayLoc != null) dieuKienLocObj.NgayLoc = ((DateTime)ConvertUtil.ToDateTime(dieuKienLocObj.NgayLoc)).ToString("yyyy-MM-dd");
            var resp = new DongMoNuocOnlineDao().Leader_HuyPheDuyetTNCN(dieuKienLocObj);
            return resp;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static int BoHuyPheDuyetTNCN(string dieuKienLocStr)
        {
            var dieuKienLocObj = JsonConvert.DeserializeObject<DieuKienLoc>(dieuKienLocStr);
            if (dieuKienLocObj.NgayLoc != null) dieuKienLocObj.NgayLoc = ((DateTime)ConvertUtil.ToDateTime(dieuKienLocObj.NgayLoc)).ToString("yyyy-MM-dd");
            var resp = new DongMoNuocOnlineDao().Leader_Bo_HuyPheDuyetTNCN(dieuKienLocObj);
            return resp;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetThongBaoTNCN(string khConNoStr)
        {
            var khConNo = JsonConvert.DeserializeObject<KhConNo>(khConNoStr);
            var base64Str = new DongMoNuocOnlineDao().SetGiayThongBaoTNCN(khConNo);
            return base64Str;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static ResultLoginVnptCA LoginVnpt(string loginEntityStr)
        {
            var loginEntity = JsonConvert.DeserializeObject<LoginEntity>(loginEntityStr);
            var result = new DongMoNuocOnlineDao().LoginVnptCA(loginEntity);
            if (result.token == null)
            {
                throw new Exception("Sai password");
            }
            return result;
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string Sign(string docSignStr, string khConNoStr, string loginName)
        {
            var docSign = JsonConvert.DeserializeObject<doc_sign>(docSignStr);
            var khConNo = JsonConvert.DeserializeObject<KhConNo>(khConNoStr);

            var result = new DongMoNuocOnlineDao().Sign(docSign, khConNo, loginName);
            return result;
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static string KyDuyetTNCN(string idkh, int ky, int nam, string xncn, string tenkh, string diaChi, string ngayNhap)
        //{
           
        //    var base64Str = new DongMoNuocOnlineDao().GetBase64PDFDuyetTNCN(idkh, ky, nam, xncn, tenkh, diaChi, ngayNhap);
        //    return base64Str;
        //}
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public static bool CapNhatTBTNCN(string idkh, int ky, int nam, string base64Str)
        //{
        //    var result = new DongMoNuocOnlineDao().CapNhatDuyetCupNuoc(idkh, ky, nam, base64Str);
        //    if (!result) throw new Exception();
        //    return true;
        //}
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