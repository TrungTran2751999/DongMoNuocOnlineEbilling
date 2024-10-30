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

namespace EOSCRM.Web.Forms.KhachHang.BaoCao
{
    public partial class DongMoNuocOnline : Authentication
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
                    
                    if (HttpContext.Current.Session["dataTBQHTN" + id] != null)
                    {
                        //HttpCookie isDowload = new HttpCookie("isDowload", "");
                        //isDowload.Expires = DateTime.Now.AddDays(-1);
                        string dataTBQHTN = HttpContext.Current.Session["dataTBQHTN" + id].ToString();
                        HttpContext.Current.Session["dataTBQHTN" + id] = null;

                        var listInfoTBQH = JsonConvert.DeserializeObject<List<InfoTB>>(dataTBQHTN);
                        //string contentTB = _dongMoNuocOnlineDao.TaoThongBaoQuaHanTienNuoc(originFile, listInfoTBQH);
                        string originTBQHTN = originFile + @"BienBan\ThongBaoQuaHanTienNuoc.docx";
                        byte[] fileBytes = _dongMoNuocOnlineDao.TaoThongBaoQuaHanTienNuocNotZipNotCreate(originTBQHTN, listInfoTBQH, originFile);
                        //byte[] fileBytes = _dongMoNuocOnlineDao.TaoThongBaoQuaHanTienNuocNotZip(originTBQHTN, listInfoTBQH, currentTBQHTN);
                        //byte[] fileBytes = Encoding.UTF8.GetBytes(contentTB);

                        Response.Clear();
                        //Response.Cookies.Remove("isDowload");
                        //Response.Cookies.Add(isDowload);
                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("Content-Disposition", "attachment; filename=TBQHTN.docx");
                        Response.BinaryWrite(fileBytes);
                        Response.Flush();
                        Response.End();
                    }
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
            var GDXN = _dongMoNuocOnlineDao.CheckGDXN(nameLogin);
            if (GDXN == null)
            {
                containerBtnQuaHan.Visible = false;
                containerBtnTamNgungCapNuoc.Visible = false;
            }
            else
            {
                MAPB = GDXN.MAPB;
                checkGDXN = true;
            }

            var now = DateTime.Now.AddMonths(-1);
            
            txtKYHD.Text = now.ToString(ConfigDao.Value(DATEFORMAT.F_MMYYYY.ToString()));
            txtNgayTBQHTN.Text = DateTime.Now.ToString(ConfigDao.Value(DATEFORMAT.F_DDMMYYYY.ToString()));
            txtNgayTNCN.Text = DateTime.Now.ToString(ConfigDao.Value(DATEFORMAT.F_DDMMYYYY.ToString()));

            var txtKYHDText = "01/" + txtKYHD.Text.Trim();
            namHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Year;
            thangHD = ConvertUtil.ToDateTime(txtKYHDText.Trim()).Value.Month;

            ddlTT.Items.Clear();
            ddlTT.Items.Add(new ListItem("", ""));
            ddlTT.Items.Add(new ListItem("TB quá hạn tiền nước", "TBQHTN"));
            ddlTT.Items.Add(new ListItem("Tạm ngừng cấp nước", "TNCN"));

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

            udpNgayTBQHTN.Visible = false;
            udpNgayTNCN.Visible = false;
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
            var TTSlected = ddlTT.SelectedValue != "" ? ddlTT.SelectedValue : null;
            
            var xncnSelected = ddlXNCN.SelectedValue != "" ? ddlXNCN.SelectedValue : null;
            var ngayTBQH = txtNgayTBQHTN.Text != "" && txtNgayTBQHTN .Text!=null? ConvertUtil.ToDateTime(txtNgayTBQHTN.Text) : null;

            //var myds = new ReportClass().KH_LocKHChuaTT(thang, nam, tenDuongPho, kvList, idkh, TTSlected, xncnSelected, ngayTBQH);
            //if (myds == null || myds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            //lay du lieu ve thong bao qua han tien nuoc
            //if (TTSlected == "TBQHTN" && xncnSelected!=null && myds.Tables[0].Rows.Count > 0)
            //{
            //    upnlTBQHTN.Visible = true;
            //    listInfoPh = new List<InfoTB>();
            //    tenGDXN = _dongMoNuocOnlineDao.GetTenGDXN(xncnSelected); 
            //    foreach (DataRow row in myds.Tables[0].Rows)
            //    {
            //        try
            //        {
            //            var infoTB = new InfoTB();
            //            infoTB.Idkh = (string)row["IDKH"];
            //            infoTB.M3TinhTien = (long)Convert.ToDouble(row["M3TINHTIEN"]);
            //            infoTB.TenKH = (string)row["TENKH"];
            //            infoTB.TongTien = (long)Convert.ToDouble(row["TONGTIEN"]);
            //            infoTB.DiaChi = row["DIACHI"] != DBNull.Value && row["DIACHI"] != null ? (string)row["DIACHI"] : "";
            //            infoTB.Ky = (int)Convert.ToDouble(row["THANG"]);
            //            infoTB.Nam = (int)Convert.ToDouble(row["NAM"]);
            //            infoTB.NgayNhap = (DateTime)Convert.ToDateTime(row["NGAYNHAP"]);
            //            infoTB.XNCN = xncnSelected;
            //            infoTB.GDXNCN = tenGDXN;
            //            listInfoPh.Add(infoTB);
            //        }
            //        catch
            //        {
            //            Console.WriteLine(listInfoPh.Count);
            //        }
            //    }

            //    var listTBQHTN = JsonConvert.SerializeObject(listInfoPh);
            //    id = "" + GuidRandom + GuidRandom;
            //    HttpContext.Current.Session["dataTBQHTN" + id] = listTBQHTN;
            //    HttpCookie idUuid = new HttpCookie("idUuid", id);
            //    Response.Cookies.Add(idUuid);
            //}
            //else
            //{
            //    upnlTBQHTN.Visible = false;
            //}
            //var ds = new ReportDataSource("dsKhachHangChuaTT", myds.Tables[0]);


            //rpViewer.LocalReport.DataSources.Clear();
            //rpViewer.LocalReport.ReportPath = "Reports/QuanLyKhachHang/DanhSachKhachHangChuaTT.rdlc";
            //rpViewer.LocalReport.DataSources.Add(ds);

            var title = "";
            if (ddlTT.SelectedValue == "")
            {
                title = String.Format("DANH SÁCH KHÁCH HÀNG CHƯA THANH TOÁN KỲ {0}/{1}", thang, nam);
            }
            else if (ddlTT.SelectedValue == "TBQHTN")
            {
                title = String.Format("DANH SÁCH KHÁCH HÀNG ĐÃ DUYỆT THÔNG BÁO QUÁ HẠN TIỀN NƯỚC CHƯA THANH TOÁN KỲ {0}/{1}", thang, nam);
            }
            else if (ddlTT.SelectedValue == "TNCN")
            {
                title = String.Format("DANH SÁCH KHÁCH HÀNG ĐÃ DUYỆT TẠM NGƯNG CẤP NƯỚC KỲ {0}/{1}", thang, nam);
            }
            var rpThang = new ReportParameter("rpThang", thang.ToString());
            var rpNam = new ReportParameter("rpNam", nam.ToString());
            //var total = new ReportParameter("total", myds.Tables[0].Rows.Count.ToString());
            var titleRp = new ReportParameter("title", title);
            //rpViewer.LocalReport.SetParameters(new[] { rpThang, rpNam, total, titleRp });


            rpViewer.LocalReport.Refresh();
            divCR.Visible = true;
            upnlReport.Update();
           

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
            //var result = _dongMoNuocOnlineDao.PheDuyetQuaHanTienNuoc(namHD, thangHD, GDXN.MAPB);
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
            if (ddlTT.SelectedValue == "TBQHTN")
            {
                udpNgayTBQHTN.Visible = true;
                udpNgayTNCN.Visible = false;
            }
            else if (ddlTT.SelectedValue == "TNCN")
            {
                udpNgayTBQHTN.Visible = false;
                udpNgayTNCN.Visible = true;
            }
            else
            {
                udpNgayTBQHTN.Visible = false;
                udpNgayTNCN.Visible = false;
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