<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
	CodeBehind="Leader_DuyetTBQHTN_2.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.BaoCao.Leader_DuyetTBQHTN_2" %>
<%@ Register TagPrefix="eoscrm" Namespace="EOSCRM.Controls" Assembly="EOSCRM.Controls" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.5.40412.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server" >
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="mainCPH" runat="server">
    <script src="../../content/scripts/exportExel.js"></script>
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
														Text='<%# Eval("MADP") + 
																				(Eval("DUONGPHU").ToString() == "" ? "" : " - " + Eval("DUONGPHU")) %>'></eoscrm:LinkButton>
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
															<input id="chkKVNV" class="checkbox-khu-vuc" title='<%# Eval("MAKV") %>' runat="server" type="checkbox" />
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
                                <%--<asp:UpdatePanel runat="server" ID="udpNgayTBQHTN">
                                    <ContentTemplate>
                                        <div class ="left" style="margin-left:100px">
									        <div class ="right">Ngày TBQHTN</div>
								        </div>
								        <div class="left width-100">
									        <eoscrm:TextBox ID="txtNgayTBQHTN" runat="server" tag="datevn" />
								        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel runat="server" ID="udpNgayTNCN">
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
							<td class="crmcell" style="display:flex">
								<div class="left">
									<eoscrm:Button ID="btnBaoCao" runat="server" OnClick="btnBaoCao_Click" TabIndex="8" CssClass="report"/>
								</div>
                                <div class="left">
                                    <%--<asp:UpdatePanel runat="server" ID="containerBtnQuaHan">
                                        <ContentTemplate>
                                            <button type="button" onclick="onDuyetTBQuaHan()">Duyệt thông báo quá hạn</button>
                                            <div style="display:none">
                                                <eoscrm:Button ID="btnDuyetTBQuaHan" runat="server" Text="Duyệt thông báo quá hạn" OnClick="btnDuyetTBQuaHan_Click" TabIndex="8" BorderColor="Black" BorderWidth="2px" CssClass="bold" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
								</div>
                                <%--<div class="left">
                                    <asp:UpdatePanel runat="server" ID="containerBtnTamNgungCapNuoc">
                                        <ContentTemplate>
                                            <button type="button" onclick="onDuyetTamNgungCapNuoc()">Duyệt tạm ngừng cấp nước</button>
                                            <div style="display:none">
                                                <eoscrm:Button ID="btnTamNgungCapNuoc" runat="server" Text="Duyệt tạm ngừng cấp nước" OnClick="btnDuyetTNCapNuoc_Click" TabIndex="8" BorderColor="Black" BorderWidth="2px" CssClass="bold" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
								</div>--%>
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	<br />
     <asp:UpdatePanel ID="UptPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="crmcontainer" id="contNhacNo" runat="server" visible="false">  
                <button id="btnNhacNoExcel" onclick="pheDuyetQuaHanLan2()">Duyệt thông báo quá hạn lần 2</button> 
                <a href="../../content/template/mau_huy_phe_duyet_hang_loat.xlsx">Tải mẫu excel duyệt hàng loạt</a>
                <label for="upload" style="float:right; background-color: #8181e3; color:white" class="custom-upload">
                    Tải lên file Excel để hủy phê duyệt hàng loạt 
                </label>     
                <input onchange="huyPheDuyetHangLoatByExcel(event)" style="display:none" id="upload" style="float:right" type="file" accept=".xlsx, .xls"/>
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
                                   <input id="<%# Eval("Idkh") + new Random().Next(0,1000000).ToString() %>" type="checkbox" onchange="onSelectedCheckBox('<%# Eval("Idkh") %>', '<%# Eval("M3TinhTien") %>', '<%# Eval("TongTien") %>', this,  '<%# Eval("NgayThongBaoNhacNoStr") %>', '<%# Eval("SoDienThoai")??"" %>')" />
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

                         <asp:TemplateField HeaderText="Trạng thái" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# Eval("ManagerDuyetTBQH2") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="" HeaderStyle-Width="80px">
                            <ItemTemplate>
                               <a id="huyPheDuyetBtn-<%# Eval("Idkh") %>" href="#" onclick="huyPheDuyetQuaHanLan2('<%# Eval("Idkh") %>')"><%# Eval("LabelHuyPheDuyet") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="" HeaderStyle-Width="80px">
                            <ItemTemplate>
                               <a id="boHuyPheDuyetBtn-<%# Eval("Idkh") %>" href="#" onclick="boHuyPheDuyetQuaHanLan2('<%# Eval("Idkh") %>')"><%# Eval("LabelBoHuyPheDuyet") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
	 <asp:UpdatePanel ID="upnlReport" UpdateMode="Conditional" runat="server">
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
	</asp:UpdatePanel>

    <script>
        const loginName = "<%=loginName%>"
        let listIdKHNhacNo = []
        let listIdKHHuyNhacNo = []
        let listIdkhBoHuyNhacNo = []
        let huyPheDuyetBtn = $(".huyPheDuyetBtn");
        function onSelectedCheckBox(idkh, m3TieuThu, tongTien, checkBox, ngayThanhToan, soDienThoai) {
            let obj = {
                idkh: idkh,
                m3TieuThu: m3TieuThu,
                tongTien: tongTien,
                ngayThanhToan: ngayThanhToan,
                soDienThoai: soDienThoai
            }
            if ($(checkBox).is(":checked")) {
                listIdKHNhacNo.push(idkh)
            } else {
                for (let i = 0; i < listIdKHNhacNo.length; i++) {
                    if (obj.idkh == listIdKHNhacNo[i]) {
                        listIdKHNhacNo.splice(i, 1)
                    }
                }
            }

        }
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
                if ($(listKV[i]).is(":selected")) {
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
                NamHd: +kyHd.split("/")[1],
                TenDuongPho: tenDuongPho || null,
                KhuVuc: tenKV || allKV,
                Idkh: $("#ctl00_mainCPH_txtIDKH").val() || null,
                XNCN: xncn || null,
                MaDuongPho: $("#ctl00_mainCPH_ddlLoTrinh").val() || null,
                NgayLoc: $("#ctl00_mainCPH_txtNgayLoc").val() || null,
                TrangThai: trangThai
                //ngayTBQH: ngayTBQHTN
            }
            return objectLoc
        }
        function callApiDuyetTBQH2() {
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
                    ListIDKH: listIdKHNhacNo,
                    isGetAll: $("#chkAllTop").is(":checked")
                }
            )
            return $.ajax({
                url: "/Forms/DongMoNuocOnline/Leader_DuyetTBQHTN_2.aspx/PheDuyetThongBaoQuaHanLan2",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ dieuKienLocStr: data, loginName: loginName }),
                async: true,
            })
        }
        function callApiHuyPheDuyetTBQH2() {
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
                    ListIDKH: listIdKHHuyNhacNo,
                    isGetAll: $("#chkAllTop").is(":checked")
                }
            )
            return $.ajax({
                url: "/Forms/DongMoNuocOnline/Leader_DuyetTBQHTN_2.aspx/HuyPheDuyetThongBaoQuaHanLan2",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ dieuKienLocStr: data}),
                async: true,
            })
        }
        function callApiBoHuyPheDuyetTBQH2() {
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
                    ListIDKH: listIdkhBoHuyNhacNo,
                    isGetAll: $("#chkAllTop").is(":checked")
                }
            )
            return $.ajax({
                url: "/Forms/DongMoNuocOnline/Leader_DuyetTBQHTN_2.aspx/BoHuyPheDuyetThongBaoQuaHanLan2",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ dieuKienLocStr: data }),
                async: true,
            })
        }

        function getAllKhachHangNo() {
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
                        NgayLoc: filter.NgayLoc
                    }
                )
            
            return $.ajax({
                url: "/Forms/DongMoNuocOnline/Leader_DuyetTBQHTN_2.aspx/GetListTBQHLan2",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ dieuKienLocStr: data }),
                async: true,
            })

        }
        function pheDuyetQuaHanLan2() {
            let isCheckAll = $("#chkAllTop").is(":checked");
            if (isCheckAll) {
                openWaitingDialog();
                listIdKHNhacNo = []
                //getAllKhachHangNo()
                //.done(function (res) {
                //    let listKh = res.d
                //    for (let i = 0; i < listKh.length; i++) {
                //        listIdKHNhacNo.push(listKh[i].Idkh)
                //    }
                //    callApiDuyetTBQH2()
                //    .done(function (res) {
                //        showInfor("Phê duyệt thông báo quá hạn lần 2 thành công")
                //        $("#ctl00_mainCPH_btnBaoCao").click()
                //        listIdKHNhacNo = []
                //        closeWaitingDialog()
                //    })
                //    .fail(function (res) {
                //        showError("Phê duyệt không thành công")
                //        listIdKHNhacNo = []
                //        closeWaitingDialog()
                //    })
                //})
                //.fail(function (err) {
                //    closeWaitingDialog()
                //})
                callApiDuyetTBQH2()
                    .done(function (res) {
                        showInfor("Phê duyệt thông báo quá hạn lần 2 thành công")
                        $("#ctl00_mainCPH_btnBaoCao").click()
                        listIdKHNhacNo = []
                        closeWaitingDialog()
                    })
                .fail(function (e) {
                    showError("Phê duyệt thông báo quá hạn lần 2 không thành công")
                    closeWaitingDialog()
                    console.log(e)
                })
            } else {
                if (listIdKHNhacNo.length > 0) {
                    openWaitingDialog();
                    callApiDuyetTBQH2()
                    .done(function (res) {
                        showInfor("Phê duyệt thông báo quá hạn lần 2 thành công")
                        $("#ctl00_mainCPH_btnBaoCao").click()
                        //for (let i = 0; i < listIdKHNhacNo.length; i++) {
                        //    let idkh = listIdKHNhacNo[i];
                        //    $("#huyPheDuyetBtn-" + idkh).text("Hủy phê duyệt")
                        //}
                        listIdKHNhacNo = []
                        closeWaitingDialog()
                    })
                    .fail(function (res) {
                        showError("Phê duyệt không thành công")
                        listIdKHNhacNo = []
                        closeWaitingDialog()
                    })
                } else {
                    showWarning("Không có khách hàng nào được chọn")
                }
            }
        }
        function huyPheDuyetQuaHanLan2(idkh) {
            listIdKHHuyNhacNo.push(idkh)
            openWaitingDialog();
            callApiHuyPheDuyetTBQH2()
            .done(function () {
                showInfor("Hủy phê duyệt thông báo quá hạn lần 2 thành công")
                $("#ctl00_mainCPH_btnBaoCao").click()
                closeWaitingDialog();
                listIdKHHuyNhacNo = []
            })
            .fail(function () {
                showError("Hủy phê duyệt không thành công")
                listIdKHHuyNhacNo = []
            })
        }
        function huyPheDuyetHangLoatByExcel(event) {
            const file = event.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    const data = new Uint8Array(e.target.result);
                    const workbook = XLSX.read(data, { type: 'array' });

                    // Assuming you want the first sheet
                    const firstSheetName = workbook.SheetNames[0];
                    const worksheet = workbook.Sheets[firstSheetName];

                    // Convert to JSON
                    const json = XLSX.utils.sheet_to_json(worksheet);
                    listIdKHHuyNhacNo = []
                    for (let i = 0; i < json.length; i++) {
                        listIdKHHuyNhacNo.push(json[i]["IDKH"])
                    }
                    console.log(json)
                    openWaitingDialog();
                    callApiHuyPheDuyetTBQH2()
                    .done(function () {
                        showInfor("Hủy phê duyệt thông báo quá hạn lần 2 thành công")
                        $("#ctl00_mainCPH_btnBaoCao").click()
                        
                        listIdKHHuyNhacNo = []
                    })
                    .fail(function () {
                        showError("Hủy phê duyệt không thành công")
                        listIdKHHuyNhacNo = []
                        closeWaitingDialog();
                    })
                }
                reader.readAsArrayBuffer(file);
            }
        }
        function boHuyPheDuyetQuaHanLan2(idkh) {
            listIdkhBoHuyNhacNo.push(idkh)
            openWaitingDialog();
            callApiBoHuyPheDuyetTBQH2()
            .done(function () {
                showInfor("Bỏ hủy phê duyệt thông báo quá hạn lần 2 thành công")
                $("#ctl00_mainCPH_btnBaoCao").click()
                closeWaitingDialog();
                listIdkhBoHuyNhacNo = []
            })
            .fail(function () {
                showError("Bỏ hủy phê duyệt không thành công")
                listIdkhBoHuyNhacNo = []
            })
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