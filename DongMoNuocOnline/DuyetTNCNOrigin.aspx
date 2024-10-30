<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
	CodeBehind="DuyetTNCNOrigin.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.BaoCao.DuyetTNCNOrigin" %>
<%@ Register TagPrefix="eoscrm" Namespace="EOSCRM.Controls" Assembly="EOSCRM.Controls" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.5.40412.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server" >
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="mainCPH" runat="server">
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
															<input id="chkKVNV" title='<%# Eval("MAKV") %>' runat="server" type="checkbox" />
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
									<asp:DropDownList ID="ddlTT" runat="server" AutoPostBack="true" ViewStateMode="Enabled" EnableViewState="true" OnSelectedIndexChanged="ddlTT_SelectedIndexChanged" onchange="onchangeTrangThai()"/>
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
                                <div id="containerTBQHTN" style="display:none" runat="server">
                                    <asp:UpdatePanel runat="server" ID="udpNgayTBQHTN">
                                        <ContentTemplate>
                                            <div class ="left" style="margin-left:100px">
									            <div class ="right">Ngày TBQHTN</div>
								            </div>
								            <div class="left width-100">
									            <eoscrm:TextBox ID="txtNgayTBQHTN" runat="server" tag="datevn" />
								            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                
                                <div id="containerTNCN" style="display:none" runat="server">
                                    <asp:UpdatePanel runat="server" ID="udpNgayTNCN">
                                        <ContentTemplate>
                                            <div class ="left" style="margin-left:100px">
									            <div class ="right">Ngày TNCN</div>
								            </div>
								            <div class="left width-100">
									            <eoscrm:TextBox ID="txtNgayTNCN" runat="server" tag="datevn" />
								            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
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
							<td class="crmcell right"></td>
							<td class="crmcell">
								<div class="left">
									<eoscrm:Button ID="btnBaoCao" runat="server" OnClick="btnBaoCao_Click" TabIndex="8" CssClass="report"/>
								</div>
                                <div class="left">
                                    <asp:UpdatePanel runat="server" ID="containerBtnQuaHan">
                                        <ContentTemplate>
                                            <button type="button" onclick="onDuyetTBQuaHan()">Duyệt thông báo quá hạn</button>
                                            <div style="display:none">
                                                <eoscrm:Button ID="btnDuyetTBQuaHan" runat="server" Text="Duyệt thông báo quá hạn" OnClick="btnDuyetTBQuaHan_Click" TabIndex="8" BorderColor="Black" BorderWidth="2px" CssClass="bold" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
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
     <%--table--%>
    <asp:UpdatePanel ID="upnlDuyetTNCN" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="crmcontainer" id="divDuyetTNCN" runat="server" >               
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowCommand="gvList_RowCommand"
                    OnPageIndexChanging="gvList_PageIndexChanging" OnRowDataBound="gvList_RowDataBound"
                    PageSize="20">
                    <PagerSettings FirstPageText="hợp đồng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                            </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

	 <asp:UpdatePanel ID="upnlReport" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
			<div class="crmcontainer" id="divCR" runat="server">
                
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
        //$("#ctl00_mainCPH_ddlTT").on("change", function () {
        //    console.log("hihi")
        //    onchangeTrangThai()
        //})
        const searchParams = new URLSearchParams(window.location.search);
        const id = searchParams.get('id');
        if (id == "TBQHTN") {
            $("#containerTBQHTN").css("display", "")
            $("#containerTNCN").css("display", "none")
        } else if (id == "TNCN") {
            $("#containerTBQHTN").css("display", "none")
            $("#containerTNCN").css("display", "")
        }
        function onchangeTrangThai() {
            let valSelected = $("#ctl00_mainCPH_ddlTT").val()
            window.history.pushState({}, '', '?id=' + valSelected);
            const searchParams = new URLSearchParams(window.location.search);
            const id = searchParams.get('id');
            
            if (id == "TBQHTN") {
                $("#containerTBQHTN").css("display", "")
                $("#containerTNCN").css("display", "none")
            } else if (id == "TNCN") {
                $("#containerTBQHTN").css("display", "none")
                $("#containerTNCN").css("display", "")
            }
        }
    </script>
</asp:Content>