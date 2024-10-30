<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
	CodeBehind="XemThongTinDongMoNuoc.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.BaoCao.XemThongTinDongMoNuoc" %>
<%@ Register TagPrefix="eoscrm" Namespace="EOSCRM.Controls" Assembly="EOSCRM.Controls" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.5.40412.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server" >
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="mainCPH" runat="server">
    <script src="../../content/scripts/exportExel.js"></script>
    <style>
        .btn-phan-trang{
            border: 1px solid black;
            padding: 6px;
        }
        .btn-phan-trang:hover{
            background: #00ffba33;
            cursor: pointer
        }
        .active{
            background: cyan;
        }
    </style>
	<div id="divDuongPho" style="display: none">
		<asp:UpdatePanel ID="upnlDuongPho" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<table class="crmtable width-500">
					<tbody>
						<tr class="crmcontainer">
							<td class="crmcell right">
								Từ khóa
							</td>
							<td class="crmcell">
								<div class="left">
									<eoscrm:TextBox ID="txtFilterDP" PostBackControlID="btnFilterDP" runat="server" Width="250px" MaxLength="200" />
								</div>
								<div class="left">
									<eoscrm:Button ID="btnFilterDP" OnClick="btnFilterDP_Click" runat="server" CssClass="filter" />
								</div>
							</td>
						</tr>
						<tr>
							<td class="crmcell ptop-10" colspan="2">
								<div class="crmcontainer">
									<eoscrm:Grid ID="gvDuongPho" runat="server" OnRowCommand="gvDuongPho_RowCommand" UseCustomPager="true">
										<PagerSettings FirstPageText="lộ trình" PageButtonCount="2" />
										<Columns>
											<asp:TemplateField HeaderStyle-Width="40px" HeaderText="Mã ĐP">
												<ItemTemplate>
													<eoscrm:LinkButton ID="lnkBtnID" runat="server" 
														CommandArgument='<%# Eval("MADP") + "-" + Eval("DUONGPHU") %>' 
														CommandName="SelectMADP" 
														Text='<%# Eval("MADP") + (Eval("DUONGPHU").ToString() == "" ? "" : " - " + Eval("DUONGPHU")) %>'></eoscrm:LinkButton>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="TENDP" HeaderText="Tên lộ trình" />
											<asp:TemplateField HeaderStyle-Width="100px" HeaderText="Chi nhánh">
												<ItemTemplate>
													<%# Eval("KHUVUC.TENKV") %>
												</ItemTemplate>
											</asp:TemplateField>
										</Columns>
									</eoscrm:Grid>
								</div>
							</td>
						</tr>
					</tbody>
				</table>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
	<asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
			<div class="crmcontainer">
				<table class="crmtable">
					<tbody>
						<tr>
							<td class="crmcell right">Kỳ hóa đơn</td>
							<td class="crmcell">
								<div class="left width-100">
									<asp:TextBox ID="txtKYHD" runat="server" tag="datemmyy" TabIndex="1"/>
								</div>
								<div class ="left" style="margin-left:100px">
									<div class ="right">IDKH</div>
								</div>
								<div class="left width-100">
									<eoscrm:TextBox ID="txtIDKH" runat="server" tag="" />
								</div>
							</td>
                            <td class="crmcell" rowspan="3">
								<div class="scrollList" style="width: 450px;">
									<asp:UpdatePanel runat="server" ID="uprptKhuvuc" UpdateMode="Conditional">
										<ContentTemplate>
											<asp:DataList ID="dlKHUVUC" runat="server" RepeatColumns="3" RepeatDirection="Vertical" RepeatLayout="Table">
												<ItemTemplate>
													<table class="p-0" width="150px">
													<tr>
														<td style="width: 5%; border: 0px;">
															<input class="checkbox-khu-vuc" id="chkKVNV" title='<%# Eval("MAKV") %>' runat="server" type="checkbox" />
														</td>
														<td style="width: 95%; border: 0px;">
															<%# Eval("TENKV") %>
														</td>
													</tr>
													</table>
												</ItemTemplate>
											</asp:DataList>
										</ContentTemplate>
									</asp:UpdatePanel>
								</div>
							</td>
						</tr> 
						<tr>
							<td class="crmcell right" style="width:20px">Trạng thái</td>
							<td class="crmcell">
								<div class="left width-100">
									<asp:DropDownList ID="ddlTT" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTT_SelectedIndexChanged"/>
								</div>

                                <%--<div id="containerTBQHTN" style="display:none">
                                    <div class ="left" style="margin-left:100px">
									    <div class ="right">Ngày TBQHTN</div>
								    </div>
								    <div class="left width-100">
									    <eoscrm:TextBox ID="txtNgayTBQHTN" runat="server" tag="datevn" />
								    </div>
                                </div>
                                <div id="containerTNCN" style="display:none">
                                    <div class ="left" style="margin-left:100px">
									    <div class ="right">Ngày TNCN</div>
								    </div>
								    <div class="left width-100">
									    <eoscrm:TextBox ID="txtNgayTNCN" runat="server" tag="datevn" />
								    </div>
                                </div>--%>
                                <asp:UpdatePanel runat="server" ID="udpIsZaloAndApp">
                                    <ContentTemplate>
                                        <div class ="left" style="margin-left:100px">
									        <div class ="right">Cài zalo/app</div>
								        </div>
								        <div class="left width-100">
									        <asp:DropDownList ID="ddlZaloAndApp" runat="server" AutoPostBack="true"/>
								        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <%--<asp:UpdatePanel runat="server" ID="udpNgayTNCN">
                                    <ContentTemplate>
                                        <div class ="left" style="margin-left:100px">
									        <div class ="right">Ngày TNCN</div>
								        </div>
								        <div class="left width-100">
									        <eoscrm:TextBox ID="txtNgayTNCN" runat="server" tag="datevn" />
								        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>--%>
							</td>                      
						</tr>   
						<tr>
                          <td class="crmcell right">XNCN</td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:DropDownList ID="ddlXNCN" runat="server" />
                                </div>
                                <div class ="left" style="margin-left:100px">
									<div class ="right">Đường phố</div>
								</div>
								<div class="left width-100">
									<eoscrm:TextBox ID="txtDuongPho" runat="server" tag="" />
								</div>
                            </td>                            
                        </tr> 
                        <tr>
                          <td class="crmcell right">Ngày lọc</td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:TextBox ID="txtNgayLoc" runat="server" tag="datevn" />
                                </div>
                                <div class ="left" style="margin-left:100px">
									<div class ="right">Lộ trình</div>
								</div>
								<div class="left width-100">
									<asp:DropDownList ID="ddlLoTrinh" runat="server" />
								</div>
                            </td>                            
                        </tr> 
						<tr>                    
							<td class="crmcell right"></td>
							<td class="crmcell">
								<div class="left">
									<eoscrm:Button ID="btnBaoCao" runat="server" OnClick="btnBaoCao_Click" TabIndex="8" CssClass="report"/>
								</div>
                               <%-- <div class="left">
									<button onclick="getDanhSachTBQHTN()">Tải thông báo quá hạn tiền nước</button>
								</div>
                                <div class="left">
									<button onclick="getDanhSachTNCN()">Tải thông báo tạm ngừng cấp nước</button>
								</div>--%>
                                <%--<div class="left">
                                    <asp:UpdatePanel runat="server" ID="containerBtnQuaHan">
                                        <ContentTemplate>
                                            <button type="button" onclick="onDuyetTBQuaHan()">Duyệt thông báo quá hạn</button>
                                            <div style="display:none">
                                                <eoscrm:Button ID="btnDuyetTBQuaHan" runat="server" Text="Duyệt thông báo quá hạn" OnClick="btnDuyetTBQuaHan_Click" TabIndex="8" BorderColor="Black" BorderWidth="2px" CssClass="bold" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
								</div>
                                <div class="left">
                                    <asp:UpdatePanel runat="server" ID="containerBtnTamNgungCapNuoc">
                                        <ContentTemplate>
                                            <button type="button" onclick="onDuyetTamNgungCapNuoc()">Duyệt tạm ngừng cấp nước</button>
                                            <div style="display:none">
                                                <eoscrm:Button ID="btnTamNgungCapNuoc" runat="server" Text="Duyệt tạm ngừng cấp nước" OnClick="btnDuyetTNCapNuoc_Click" TabIndex="8" BorderColor="Black" BorderWidth="2px" CssClass="bold" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
								</div>--%>
                                <div class="left">
                                    
								</div>
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	<br />
	 <%--<asp:UpdatePanel ID="upnlReport" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
			<div class="crmcontainer" id="divCR" runat="server" visible="false">
                <asp:UpdatePanel runat="server" ID="upnlTBQHTN">
                    <ContentTemplate>
                        <button type="button" onclick="onXuatTBQHTN()">Xuất thông báo quá hạn tiền nước</button>
                        <div style="display:none">
                            <eoscrm:Button ID="btnXuatTBQHTN" runat="server" Text="Xuất thông báo quá hạn tiền nước" OnClick="btnXuatTBQHTN_Click" TabIndex="8" BorderColor="Black" BorderWidth="2px" CssClass="bold" />
                        </div>               
                    </ContentTemplate>
                </asp:UpdatePanel>

				<rsweb:ReportViewer ID="rpViewer" runat="server" Font-Names="Verdana" 
					Font-Size="8pt" InteractiveDeviceInfos="(Collection)" ShowParameterPrompts="true"
					AsyncRendering="false" WaitMessageFont-Names="Verdana" 
					 WaitMessageFont-Size="14pt" Height="1000px" SizeToReportContent="True" 
					 Width="800px" >
				</rsweb:ReportViewer>
			</div>            
		</ContentTemplate>
	</asp:UpdatePanel>--%>

    <asp:UpdatePanel ID="UptPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="crmcontainer" id="contNhacNo" runat="server" visible="false">  
                <div style="display: flex; gap:15px">
                    <div id="containerXuatFileExcel" runat="server" visible="true">
                        <button id="btnNhacNoExcel" onclick="xuatThongBaoNhacNo()">Khởi tạo số liệu</button> 
                    </div>
                    
                    <div id="containerXuatFilePdf" runat="server" visible="false">
                        <button id="btnXuatFilePdf" onclick="getAllGiayThongBao()">Xem giấy báo toàn bộ</button> 
                    </div>   
                </div>
                      
                <eoscrm:Grid ID="gvListKH" runat="server" UseCustomPager="true" OnRowCommand="gvList_RowCommand"
                    OnPageIndexChanging="gvList_PageIndexChanging" OnRowDataBound="gvList_RowDataBound"
                    PageSize="20">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="10px">
                             <HeaderTemplate>
                                   <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox" onclick="CheckAllItems(this);" />
                             </HeaderTemplate>
                            <ItemTemplate>
                                   <input id="Id" runat="server" type="hidden" value='<%# Eval("Idkh") %>' />
                                   <input id="<%# Eval("Idkh") + new Random().Next(0,1000000).ToString() %>" onchange="onSelectedCheckBox('<%# Eval("Idkh") %>', '<%# Eval("M3TinhTien") %>', '<%# Eval("TongTien") %>', this,  '<%# Eval("NgayThongBaoNhacNoStr") %>', '<%# Eval("SoDienThoai")??"" %>')" name="listIds" diaChi="<%#Eval("DiaChi") ??"" %>" tenKH ="<%#Eval("TenKH")%>" xncn="<%#Eval("XNCN")%>" ngaynhap="<%#Eval("NgayNhapCSStr")%>" type="checkbox" value='<%# Eval("Idkh") %>' />
                             </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="STT" HeaderStyle-CssClass="checkbox">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="IDKH" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# Eval("Idkh") ?? ""%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tên khách hàng" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# Eval("TenKH") ?? ""%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# Eval("DiaChi") ?? ""%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Tổng tiền" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# Eval("TongTien") ?? ""%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="M3" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# Eval("M3TinhTien") ?? ""%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Ngày nhập chỉ số" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# Eval("NgayNhapCSStr") ?? ""%>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Số điện thoại" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# Eval("SoDienThoai") ?? ""%>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </eoscrm:Grid>
            </div>
            <div class="cell numeric "></div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="pdf-viewer">
       <iframe id="pdf-frame" width="100%" height="90%"></iframe>
        <div id="tieu-de-pdf-viewer"></div>
       <div id="page-pdf" style="display:flex; gap:10px; margin-top:20px; flex-wrap: wrap; width:100%">
           
       </div>
    </div>

    <script>
        let listIdKHNhacNo = []
        let listKhConNoXuatThongBao = []
        let countKh = 0
        let totalPage = <%=totalPage%>
        
        //Lay gia tri de loc
        function getGiaTriDeLoc() {
            let xncn = $("#ctl00_mainCPH_ddlXNCN").val();
            let kyHd = $("#ctl00_mainCPH_txtKYHD").val();
            let tenDuongPho = $("ctl00_mainCPH_txtDuongPho").val();
            let ngayLoc = $("ctl00_mainCPH_txtNgayLoc").val();
            let trangThai = $("#ctl00_mainCPH_ddlTT").val();
            let tenKV = "";
            let listKV = $(".checkbox-khu-vuc");
            let allKV = "";
            for (let i = 0; i < listKV.length; i++) {
                console.log($(listKV[i]).is(":selected"))
                if ($(listKV[i]).is(":checked")) {
                    tenKV += $(listKV[i]).attr("title");
                }
            }

            for (let i = 0; i < listKV.length; i++) {
                if (i == 0) {
                    allKV += $(listKV[i]).attr("title");
                } else {
                    allKV += "," + $(listKV[i]).attr("title");
                }

            }
            let objectLoc = {
                Ky: +kyHd.split("/")[0],
                KyHd: +kyHd.split("/")[0],
                NamHd: +kyHd.split("/")[1],
                TenDuongPho: tenDuongPho || null,
                KhuVuc: tenKV || allKV,
                Idkh: $("#ctl00_mainCPH_txtIDKH").val().trim() || null,
                XNCN: xncn || null,
                MaLoTrinh: $("#ctl00_mainCPH_ddlLoTrinh").val() || null,
                MaDuongPho: $("#ctl00_mainCPH_txtDuongPho").val().trim() || null,
                NgayLoc: $("#ctl00_mainCPH_txtNgayLoc").val() || null,
                TrangThai: trangThai,
                isGetAll: $("#chkAllTop").is(":checked"),
                isZaloAndApp: $("#ctl00_mainCPH_ddlZaloAndApp").val(),
                Page:-1
                //ngayTBQH: ngayTBQHTN
            }
            console.log("=======ahihi")
            console.log(objectLoc)
            console.log("=======ahihi")
            return objectLoc
        }
        function exportToExcel(data,title) {
            // Create a new workbook
            const workbook = XLSX.utils.book_new();

            // Convert the data to a worksheet
            const worksheet = XLSX.utils.json_to_sheet(data);

            // Add the worksheet to the workbook
            XLSX.utils.book_append_sheet(workbook, worksheet, 'Sheet1');

            // Generate the Excel file and trigger the download
            XLSX.writeFile(workbook, title+'.xlsx');
        }
        function getAllThongBaoNhacNo() {
            openWaitingDialog();
            let filter = getGiaTriDeLoc();
            let data = JSON.stringify(
                    {
                        KyHd: +filter.Ky,
                        NamHd: +filter.NamHd,
                        MaDuongPho: filter.TenDuongPho,
                        KhuVuc: filter.KhuVuc,
                        Idkh: filter.Idkh,
                        XNCN: filter.XNCN,
                        MaLoTrinh: filter.MaDuongPho,
                        NgayLoc: filter.NgayLoc,
                        isZaloAndApp: $("#ctl00_mainCPH_ddlZaloAndApp").val()
                    }
                )
            let api = "";
            if (filter.TrangThai == "TBNN_1") {
                api = "/Forms/DongMoNuocOnline/XemThongTinDongMoNuoc.aspx/GetListTbNhacNo"
            } else if (filter.TrangThai == "TBQH_1") {
                api = "/Forms/DongMoNuocOnline/XemThongTinDongMoNuoc.aspx/GetListTBQHLan1"
            } else if (filter.TrangThai == "TBQH_2") {
                api = "/Forms/DongMoNuocOnline/XemThongTinDongMoNuoc.aspx/GetListTBQHLan2DaPheDuyet"
            } else if (filter.TrangThai == "TBTNCN") {
                api = "/Forms/DongMoNuocOnline/XemThongTinDongMoNuoc.aspx/GetListTBTNCN"
            }

            return $.ajax({
                url: api,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ dieuKienLocStr: data }),
                async: true,
            })
           
        }
        function xuatThongBaoNhacNo() {
            let isCheckAll = $("#chkAllTop").is(":checked");
            let listData = []
            let kyHd = $("#ctl00_mainCPH_txtKYHD").val().split("/")[0]
            let namHd = $("#ctl00_mainCPH_txtKYHD").val().split("/")[1]
            let filter = getGiaTriDeLoc();

            let idkh; let soTien; let m3; let ngayThanhToan; let sdt

            let title = ""
            if (filter.TrangThai == "TBNN_1") {
                title = "DanhSachNhacNoLan1_Ky_" + kyHd + "-" + namHd
            } else if (filter.TrangThai == "TBQH_1") {
                title = "DanhSachQuaHanTienNuocLan1_Ky_" + kyHd + "-" + namHd
            } else if (filter.TrangThai == "TBQH_2") {
                title = "DanhSachQuaHanTienNuocLan2_Ky_" + kyHd + "-" + namHd
            } else if (filter.TrangThai == "TBTNCN") {
                title = "DanhSachTamNgungCapNuoc_" + kyHd + "-" + namHd
            }
            if (isCheckAll) {
                getAllThongBaoNhacNo()
                .done(function (res) {
                    res = res.d
                    for (let i = 0; i < res.length; i++) {
                        let idkh = res[i].Idkh;
                        let soTien = res[i].TongTien;
                        let m3 = res[i].M3TinhTien;
                        let ngayThanhToan = res[i].NgayThongBaoNhacNoStr
                        let sdt = res[i].SoDienThoai

                        let data = {};
                        if (sdt != null && sdt != "") {
                            data["Số điện thoại"] = sdt;
                            if (filter.TrangThai == "TBNN_1") {
                                data["Tin nhắn"] = "T/b (Lan2) KH co ID: " + idkh + ", ky " + kyHd + "/" + namHd + " tieu thu " + m3 + "m3" + " ,chua thanh toan " + soTien + "VND" + ", han thanh toan: " + ngayThanhToan + " tran trong kinh bao. TTCSKH: 1800 0036"
                            } else if (filter.TrangThai == "TBQH_1") {
                                data["Tin nhắn"] = "T/b (Lan3) KH co ID: " + idkh + ", ky " + kyHd + "/" + namHd + " tieu thu " + m3 + "m3" + " ,chua thanh toan " + soTien + "VND" + ", han thanh toan: " + ngayThanhToan + " tran trong kinh bao. TTCSKH: 1800 0036"
                            } else if (filter.TrangThai == "TBQH_2") {
                                data["Tin nhắn"] = "HueWACO Thông báo về việc quá hạn thanh toán tiền nước. Quý khách vui lòng thanh toán tiền nước kỳ " + kyHd + "/" + namHd + " trước ngày " + ngayThanhToan + "" + ". Quá thời hạn trên công ty xin phép được tạm ngừng cấp nước."
                            } else if (filter.TrangThai == "TBTNCN") {
                                data["Tin nhắn"] = "Thông báo về việc tạm ngừng dịch vụ cấp nước."
                            }

                            listData.push(data)
                        }
                        
                    }
                   
                    closeWaitingDialog();
                    if(res.length > 0) exportToExcel(listData, title)
                    listIdKHNhacNo = []
                    
                })
                .fail(function (err) {
                    closeWaitingDialog();
                });

                
            } else {
                
                for (let i = 0; i < listIdKHNhacNo.length; i++) {
                    let idkh = listIdKHNhacNo[i].idkh;
                    let soTien = listIdKHNhacNo[i].tongTien;
                    let m3 = listIdKHNhacNo[i].m3TieuThu;
                    let ngayThanhToan = listIdKHNhacNo[i].ngayThanhToan
                    let sdt = listIdKHNhacNo[i].soDienThoai
                    data = {};
                    if (sdt != null && sdt != "") {
                        if (filter.TrangThai == "TBNN_1") {
                            data["Tin nhắn"] = "T/b (Lan2) KH co ID: " + idkh + ", ky " + kyHd + "/" + namHd + " tieu thu " + m3 + "m3" + " ,chua thanh toan " + soTien + "VND" + ", han thanh toan: " + ngayThanhToan + " tran trong kinh bao. TTCSKH: 1800 0036"
                        } else if (filter.TrangThai == "TBQH_1") {
                            data["Tin nhắn"] = "T/b (Lan3) KH co ID: " + idkh + ", ky " + kyHd + "/" + namHd + " tieu thu " + m3 + "m3" + " ,chua thanh toan " + soTien + "VND" + ", han thanh toan: " + ngayThanhToan + " tran trong kinh bao. TTCSKH: 1800 0036"
                        } else if (filter.TrangThai == "TBQH_2") {
                            data["Tin nhắn"] = "HueWACO Thông báo về việc quá hạn thanh toán tiền nước. Quý khách vui lòng thanh toán tiền nước kỳ " + kyHd + "/" + namHd + " trước ngày " + ngayThanhToan + "" + ". Quá thời hạn trên công ty xin phép được tạm ngừng cấp nước."
                        } else if (filter.TrangThai == "TBTNCN") {
                            data["Tin nhắn"] = "Thông báo về việc tạm ngừng dịch vụ cấp nước."
                        }

                        listData.push(data)
                    }
                    
                }
                if (listData.length > 0) exportToExcel(listData, title)
                listIdKHNhacNo = []
            }
            
        }
        //xuat thong bao giay
        function countPageGiaThongBaoPdf(dieuKienLoc) {
            let api = ""
            if (dieuKienLoc.TrangThai == "TBQH_2") {
                api = "/Forms/DongMoNuocOnline/XemThongTinDongMoNuoc.aspx/CountGiayThongBaoQuaHan2"
            } else if (dieuKienLoc.TrangThai == "TBTNCN") {
                api = "/Forms/DongMoNuocOnline/XemThongTinDongMoNuoc.aspx/CountGiayThongBaoTamNgungCapNuoc"
            }
            return $.ajax({
                url: api,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ dieuKienLocStr: JSON.stringify(dieuKienLoc) }),
                async: true,
            })

        }
        function phanTrangThongBaoPdf(page, selector) {
            let listBtn = $(".btn-phan-trang")
            for (let i = 0; i < listBtn.length; i++) {
                $(listBtn[i]).removeClass("active")
            }
            $(selector).addClass("active")
            let filter = getGiaTriDeLoc()
            filter.Page = (page - 1) * totalPage
            filter.isGetAll = true
            let api = ""
            if (filter.TrangThai == "TBQH_2") {
                api = "/Forms/DongMoNuocOnline/XemThongTinDongMoNuoc.aspx/GetGiayThongBaoQuaHan2"
            } else if (filter.TrangThai == "TBTNCN") {
                api = "/Forms/DongMoNuocOnline/XemThongTinDongMoNuoc.aspx/GetGiayThongBaoTamNgungCapNuoc"
            }
            openWaitingDialog()
            $.ajax({
                url: api,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({listKHConNo: JSON.stringify([]), dieuKienLocStr: JSON.stringify(filter) }),
                async: true,
            })
            .done(function (res) {
                closeWaitingDialog()
                displayPDF(res.d)
                let prevPage = filter.Page + 1
                let nextPage = filter.Page + totalPage < countKh ? filter.Page + totalPage : countKh
                $("#tieu-de-pdf-viewer").html(prevPage + "-" + nextPage + " đang xem")
            })
            .fail(function (err) {
                closeWaitingDialog()
            })
        }
        function xuatThongBao(list, dieuKienLoc) {
            let listData = []
            let kyHd = $("#ctl00_mainCPH_txtKYHD").val().split("/")[0]
            let namHd = $("#ctl00_mainCPH_txtKYHD").val().split("/")[1]
            let filter = getGiaTriDeLoc();

            let idkh; let soTien; let m3; let ngayThanhToan; let sdt
            let res = list
            for (let i = 0; i < res.length; i++) {
                let idkh = res[i].Idkh;
                let soTien = res[i].TongTien;
                let m3 = res[i].M3TinhTien;
                let ngayThanhToan = res[i].NgayThongBaoNhacNoStr
                let sdt = res[i].SoDienThoai
                let tenKh = res[i].TenKH
                let ngayNhapCSStr = res[i].NgayNhapCSStr
                let diaChi = res[i].DiaChi
                let tongTien = res[i].TongTien
                let m3TinhTien = res[i].M3TinhTien
                let xncn = res[i].XNCN
                let data = {
                    Idkh: idkh,
                    Ky: kyHd,
                    Nam: namHd,
                    TenKH: tenKh,
                    NgayNhapCSStr: ngayNhapCSStr.split("/")[0] + "-" + ngayNhapCSStr.split("/")[1] + "-" + ngayNhapCSStr.split("/")[2],
                    DiaChi: diaChi,
                    XNCN: xncn,
                    TongTien: tongTien,
                    M3TinhTien: m3TinhTien
                }
                listData.push(data)
            }
            let api = ""
            if (filter.TrangThai == "TBQH_2") {
                api = "/Forms/DongMoNuocOnline/XemThongTinDongMoNuoc.aspx/GetGiayThongBaoQuaHan2"
            } else if (filter.TrangThai == "TBTNCN") {
                api = "/Forms/DongMoNuocOnline/XemThongTinDongMoNuoc.aspx/GetGiayThongBaoTamNgungCapNuoc"
            }

            return $.ajax({
                url: api,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ listKHConNo: JSON.stringify(listData), dieuKienLocStr: JSON.stringify(dieuKienLoc) }),
                async: true,
            })
        }
        function getAllGiayThongBao_1() {
            let isCheckAll = $("#chkAllTop").is(":checked");
            let listData = []
            let kyHd = $("#ctl00_mainCPH_txtKYHD").val().split("/")[0]
            let namHd = $("#ctl00_mainCPH_txtKYHD").val().split("/")[1]
            let filter = getGiaTriDeLoc();
            let idkh; let soTien; let m3; let ngayThanhToan; let sdt
            
            if (isCheckAll) {
                openWaitingDialog();
                getAllThongBaoNhacNo()
                .done(function (res) {
                    xuatThongBao(res.d)
                    .done(function (res) {
                        displayPDF(res.d)
                        $("#pdf-viewer").dialog("open");
                        closeWaitingDialog()
                    })
                    .fail(function (err) {
                        closeWaitingDialog()
                        showError("Lỗi xuất thông báo")
                        console.log(err)
                    })
                })
            } else {
                if (listIdKHNhacNo.length > 0) {
                    openWaitingDialog();
                    xuatThongBao(listKhConNoXuatThongBao)
                    .done(function (res) {
                        displayPDF(res.d)
                        $("#pdf-viewer").dialog("open");
                        closeWaitingDialog()
                    })
                    .fail(function (err) {
                        closeWaitingDialog()
                        showError("Lỗi xuất thông báo")
                        console.log(err)
                    })
                    listKhConNoXuatThongBao = []
                }
            }

        }
        function getAllGiayThongBao() {
            let isCheckAll = $("#chkAllTop").is(":checked");
            let listData = []
            let kyHd = $("#ctl00_mainCPH_txtKYHD").val().split("/")[0]
            let namHd = $("#ctl00_mainCPH_txtKYHD").val().split("/")[1]
            let filter = getGiaTriDeLoc();
            let idkh; let soTien; let m3; let ngayThanhToan; let sdt
            filter.KyHd = filter.Ky
            filter.Page = 0
            if (listIdKHNhacNo.length > 0 || isCheckAll) {
                openWaitingDialog();
                xuatThongBao(listKhConNoXuatThongBao, filter)
                .done(function (res) {
                    countPageGiaThongBaoPdf(filter)
                    .done(function (counts) {
                        $("#pdf-viewer").dialog("open");
                        displayPDF(res.d)
                        let count = counts.d.totalPage
                        countKh = counts.d.totalKh
                        console.log()
                        if (isCheckAll) {
                            let listBtnPage = "";
                            for (let i = 0; i <= count; i++) {
                                let a = i + 1
                                let buttonPage = "<div class='btn-phan-trang' onclick='phanTrangThongBaoPdf("+a+",this"+")'>" + a + "</div>";
                                listBtnPage += buttonPage;
                            }
                            $("#page-pdf").html(listBtnPage)
                            let nextPage = totalPage > countKh ? countKh : totalPage
                            console.log(countKh)
                            $("#tieu-de-pdf-viewer").html("1-" + nextPage + " đang xem")
                        } else {
                            $("#page-pdf").html("")
                            $("#tieu-de-pdf-viewer").html("")
                        }
                        
                        closeWaitingDialog()
                    })
                    .fail(function (err) {
                        console.log(err)
                        closeWaitingDialog()
                    })
                })
                .fail(function (err) {
                    closeWaitingDialog()
                    showError("Lỗi xuất thông báo")
                    console.log(err)
                })
                listKhConNoXuatThongBao = []
            }

        }

        
        function onSelectedCheckBox(idkh, m3TieuThu, tongTien, checkBox, ngayThanhToan, soDienThoai) {
            let obj = {
                idkh: idkh,
                m3TieuThu: m3TieuThu,
                tongTien: tongTien,
                ngayThanhToan: ngayThanhToan,
                soDienThoai: soDienThoai,
                diaChi: $(checkBox).attr("diaChi"),
                xncn: $(checkBox).attr("XNCN"),
            }
            let ngayNhapCSStr = $(checkBox).attr("ngaynhap");
            let objXuatThongBao = {
                Idkh: idkh,
                Ky: $("#ctl00_mainCPH_txtKYHD").val().split("/")[0],
                Nam: $("#ctl00_mainCPH_txtKYHD").val().split("/")[1],
                TenKH: $(checkBox).attr("tenKH"),
                NgayNhapCSStr: ngayNhapCSStr,
                DiaChi: $(checkBox).attr("diaChi"),
                XNCN: $(checkBox).attr("XNCN"),
                TongTien: tongTien,
                M3TinhTien: m3TieuThu
            }
            if ($(checkBox).is(":checked")) {
                listIdKHNhacNo.push(obj)
                listKhConNoXuatThongBao.push(objXuatThongBao)
            } else {
                for (let i = 0; i < listIdKHNhacNo.length; i++) {
                    if (obj.idkh == listIdKHNhacNo[i].idkh) {
                        listIdKHNhacNo.splice(i,1)
                    }
                }
                for (let i = 0; i < listKhConNoXuatThongBao.length; i++){
                    if (objXuatThongBao.Idkh == listKhConNoXuatThongBao[i].Idkh) {
                        listKhConNoXuatThongBao.splice(i, 1)
                    }
                }
            }
        }














        function onDuyetTBQuaHan() {
            let textConfirm = "Bạn có chắc chắn duyệt THÔNG BÁO QUÁ HẠN TIỀN NƯỚC danh sách các khách hàng này. Khi duyệt sẽ không thể hoàn tác. Bạn chắc chắn chứ !"
            if (confirm(textConfirm) == true) {
                $("#ctl00_mainCPH_btnDuyetTBQuaHan").click();
            } else {

            }
        }
        function onDuyetTamNgungCapNuoc() {
            let textConfirm = "Bạn có chắc chắn duyệt TẠM NGƯNG CẤP NƯỚC danh sách các khách hàng này. Khi duyệt sẽ không thể hoàn tác. Bạn chắc chắn chứ !"
            if (confirm(textConfirm) == true) {
                $("#ctl00_mainCPH_btnTamNgungCapNuoc").click();
            } else {

            }
        }
        function onXuatTBQHTN() {
            //setCookie("isDowload", "true")
            //window.history.pushState({}, '', '?id=lsdvfsdonjsdvsdovnsdov');
            $("#ctl00_mainCPH_btnXuatTBQHTN").click();
            setTimeout(function () {
                location.reload();
            }, 2000);
        }
        function setCookie(cookieName, cookieValue, expirationHours) {
            const expirationDate = new Date();
            expirationDate.setHours(expirationDate.getHours() + expirationHours);

            document.cookie = cookieName + "=" + cookieValue;
        }
        $("#pdf-viewer").dialog({
            autoOpen: false,
            modal: true,
            width: $(document).width()/2,
            height: $(document).height() / 2,
            title: "Danh sách thông báo"
        });
        function displayPDF(base64PDF) {
            // Create a Blob object from the base64 data
            const binaryData = atob(base64PDF);
            const arrayBuffer = new ArrayBuffer(binaryData.length);
            const uint8Array = new Uint8Array(arrayBuffer);

            for (let i = 0; i < binaryData.length; i++) {
                uint8Array[i] = binaryData.charCodeAt(i);
            }

            const blob = new Blob([uint8Array], { type: 'application/pdf' });

            // Create a URL for the Blob and set it as the src of an iframe
            const pdfUrl = URL.createObjectURL(blob);
            const pdfFrame = document.getElementById('pdf-frame');
            
            pdfFrame.src = pdfUrl;
        }
        function getDanhSachTBQHTN() {
            openWaitingDialog();

            let listKV = $(".checkbox-khu-vuc");
            let allKV = "";
            for (let i = 0; i < listKV.length; i++) {
                if (i == 0) {
                    allKV += $(listKV[i]).attr("title");
                } else {
                    allKV += ","+$(listKV[i]).attr("title");
                }
                
            }
            let tenKV = "";
            for (let i = 0; i < listKV.length; i++) {
                if ($(listKV[i]).is(":selected")) {
                    tenKV += $(listKV[i]).attr("title");
                }
            }
            
            let xncn = $("#ctl00_mainCPH_ddlXNCN").val();
            let kyHd = $("#ctl00_mainCPH_txtKYHD").val();
            let tenDuongPho = $("ctl00_mainCPH_txtDuongPho").val();
            let ngayTBQHTN = $("#ctl00_mainCPH_txtNgayTBQHTN").val();
            
            
            $.ajax({
                url: "/Forms/DongMoNuocOnline/XemThongTinDongMoNuoc.aspx/GetDanhSachTBQTN",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(
                    {
                        ky: +kyHd.split("/")[0],
                        nam: +kyHd.split("/")[1],
                        tenDuongPho: tenDuongPho || null,
                        kvList: tenKV || allKV,
                        idkh: $("ctl00_mainCPH_txtIDKH").val()||null,
                        xncn: xncn || null,
                        ngayTBQH: ngayTBQHTN
                    }
                ),
                async: true,
            })
           .done(function (res) {
               closeWaitingDialog();
               if (res.d == "0") {
                   alert("Không có Thông báo quá hạn tiền nước trong kì này")
               } else {
                   console.log(res.d)
                   closeWaitingDialog();
                   displayPDF(res.d);
                   listResult.push(res.d);
                   $("#pdf-viewer").dialog("open");
               }
           })
           .fail(function () {
               closeWaitingDialog();
               alert("Lấy danh sách không thành công")
           })
        }
        //let listResult = [];

        function getDanhSachTNCN() {
            openWaitingDialog();
            let xncn = $("#ctl00_mainCPH_ddlXNCN").val();
            let kyHd = $("#ctl00_mainCPH_txtKYHD").val();
            $.ajax({
                url: "/Forms/DongMoNuocOnline/XemThongTinDongMoNuoc.aspx/GetDanhSachTNCN",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify(
                    {
                        ky: +kyHd.split("/")[0],
                        nam: +kyHd.split("/")[1],
                        xncn: xncn
                    }
                ),
                async: true,
            })
           .done(function (res) {
               closeWaitingDialog();
               if (res.d == "0") {
                   alert("Không có Thông báo Tạm ngừng cấp nước trong kì này")
               } else {
                   console.log(res.d)
                   closeWaitingDialog();
                   displayPDF(res.d);
                   listResult.push(res.d);
                   $("#pdf-viewer").dialog("open");
               }
           })
           .fail(function () {
               closeWaitingDialog();
               alert("Lấy danh sách không thành công")
           })
        }
        //function getDanhSachTNCN_Test() {
        //    openWaitingDialog()
        //    for (let i = 1; i <= 2; i++) {
        //        getDanhSachTNCN_Test()
        //    }
        //    return listResult;
        //}
        //onchangeTrangThai()
        //let valSelected = $("#ctl00_mainCPH_ddlTT").val()
        //console.log(valSelected);
        //function onchangeTrangThai() {
        //    let valSelected = $("#ctl00_mainCPH_ddlTT").val()
        //    if (valSelected == "TBQHTN") {
        //        $("#containerTBQHTN").css("display", "")
        //        $("#containerTNCN").css("display", "none")
        //    } else if (valSelected == "TNCN") {
        //        $("#containerTBQHTN").css("display", "none")
        //        $("#containerTNCN").css("display", "")
        //    }
        //}
    </script>
</asp:Content>