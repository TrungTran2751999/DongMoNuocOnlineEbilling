<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
	CodeBehind="Leader_DuyetTNCN.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.BaoCao.LeaderDuyetTNCN" %>
<%@ Register TagPrefix="eoscrm" Namespace="EOSCRM.Controls" Assembly="EOSCRM.Controls" %>
<%@ Register TagPrefix="rsweb" Namespace="Microsoft.Reporting.WebForms" Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.5.40412.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server" >
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="mainCPH" runat="server">
   <%-- <script type="text/javascript" src="../content/scripts/vnpt-plugin.js"></script>
    <script type="text/javascript" src="../content/scripts/base64.js"></script>
    <script type="text/javascript" src="../content/scripts/SignContractUi.js"></script>--%>

<%--    <link rel="stylesheet" href="../../content/kyso/styles/backend/assets/ca-icon/style.css"
    <link rel="stylesheet" href="../../content/kyso/styles/backend/assets/ap8/css/style.css">
    <link rel="stylesheet" href="../../content/kyso/styles/backend/css/style.css?v+20240416">--%>
    
    <link rel="stylesheet" href="../../content/kyso/vendor/pdfsignature-2.0.0.6/asRange.min.css">
    <link rel="stylesheet" href="../../content/kyso/vendor/pdfsignature-2.0.0.6/jquery-uis.css">
    <link rel="stylesheet" href="../../content/kyso/vendor/pdfsignature-2.0.0.6/pdfsignatures.css">
    <link rel="stylesheet" href="../../content/kyso/vendor/wPaint-master/lib/wColorPicker.min.css">
    <link rel="stylesheet" href="../../content/kyso/vendor/wPaint-master/src/wPaint.css">
    <link rel="stylesheet" href="../../content/kyso/vendor/jquery-wizard/jquery-wizard.min.css">
    <link rel="stylesheet" href="../../content/kyso/vendor/dropify/dropify.min.css">
    <link rel="stylesheet" href="../../content/kyso/styles/backend/assets/customsrollbar/scrollBar.css">


    <%--<link rel="stylesheet" href="../../content/kyso/vendor/pdfsignature-2.0.0.6/asrange.min.css">
    <link rel="stylesheet" href="../../content/kyso/vendor/pdfsignature-2.0.0.6/jquery-ui.css">
    <link rel="stylesheet" href="../../content/kyso/vendor/pdfsignature-2.0.0.6/pdfsignature.css">--%>

    <script type="text/javascript" src="../../content/kyso/styles/backend/assets/jquery/popper.min.js"></script>

<%--    <script type="text/javascript" src="../../content/kyso/vendor/pdfsignature-2.0.0.6/pdfsignature.js"></script>--%>
    <script type="text/javascript" src="../../content/kyso/vendor/babel-external-helpers/babel-external-helpers.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/popper-js/umd/popper.min.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/bootstrap/bootstrap-notify.min.js"></script>
    <script type="text/javascript" src="../../content/kyso/js/moment.js"></script>
    <script type="text/javascript" src="../../content/kyso/js/Component.js"></script>

    <script type="text/javascript" src="../../content/kyso/js/Resources/setlanguage.js"></script>
    <script type="text/javascript" src="../../content/kyso/js/Resources/Signature_Resources_vi.js"></script>
    <script type="text/javascript" src="../../content/kyso/js/Resources/Signature_Resources_en.js"></script>

    <script type="text/javascript" src="../../content/kyso/vendor/icheck/icheck.min.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/dropify/dropify.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/wPaint-master/lib/jquery.ui.widget.1.10.3.min.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/wPaint-master/lib/jquery.ui.mouse.1.10.3.min.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/wPaint-master/lib/jquery.ui.core.1.10.3.min.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/wPaint-master/lib/jquery.ui.draggable.1.10.3.min.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/pdfsignature-2.0.0.6/jquery-ui.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/pdfsignature-2.0.0.6/jquery-touch.min.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/wPaint-master/lib/wColorPicker.min.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/wPaint-master/src/wPaint.utils.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/wPaint-master/src/wPaint.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/wPaint-master/plugins/main/src/wPaint.menu.main.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/pdfsignature-2.0.0.6/jquery-asRange.min.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/pdfsignature-2.0.0.6/jscolor.js"></script>
    <%--<script type="text/javascript" src="../../content/kyso/vendor/pdfsignature-2.0.0.6/pdf.js-2.6.67/pdf.worker.js"></script>--%>
    <script type="text/javascript" src="../../content/kyso/vendor/pdfsignature-2.0.0.6/pdf.js-2.6.67/pdf.js"></script>
    
    <script type="text/javascript" src="../../content/kyso/vendor/pdfsignature-2.0.0.6/pdfsignature.js"></script>
    <script type="text/javascript" src="../../content/kyso/js/filesize.js"></script>
    <script type="text/javascript" src="../../content/kyso/styles/backend/assets/customsrollbar/scrollBar.js"></script>
    <script type="text/javascript" src="../../content/kyso/js/vnpt-plugin/base64.js"></script>
    <script type="text/javascript" src="../../content/kyso/js/vnpt-plugin/vnpt-plugin.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/jquery-wizard/jquery-wizard.js"></script>
    <script type="text/javascript" src="../../content/kyso/js/signature/signature_multiple_worker.js"></script>


    <%--<script type="text/javascript" src="../../content/kyso/styles/backend/assets/customsrollbar/scrollBar.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/pdfsignature-2.0.0.6/pdf.js-2.6.67/pdf.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/pdfsignature-2.0.0.6/pdf.js-2.6.67/pdf.worker.js"></script>
    <script type="text/javascript" src="../../content/kyso/vendor/pdfsignature-2.0.0.6/jquery-asRange.min.js"></script>--%>
    <style>
        #kdc-upload{
            display:none
        }
        .signaturebox-textonly::before {
            /*background-image: url("images/default.png");*/
            background-size: contain;
            background-repeat: no-repeat;
            background-position-x: center;
            content: ' ';
            display: block;
            position: absolute;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            /*z-index: 0;*/
            opacity: 0.4;
        }
        .ui-dialog .ui-dialog-titlebar-close span { display: none; margin: 0px; }
        /*.ui-dialog{
            left: 436px; 
            top: 94px;
        }*/
    </style>
    <div id="test">
        
    </div>
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
							<%--<td class="crmcell right" style="width:20px">Trạng thái</td>
							<td class="crmcell">
								<div class="left width-100">
									<asp:DropDownList ID="ddlTT" runat="server" AutoPostBack="true" ViewStateMode="Enabled" EnableViewState="true" OnSelectedIndexChanged="ddlTT_SelectedIndexChanged" onchange="onchangeTrangThai()"/>
								</div>--%>

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
                                <%--<div id="containerTBQHTN" style="display:block" runat="server">--%>
                                    <%--<asp:UpdatePanel runat="server" ID="udpNgayTBQHTN">
                                        <ContentTemplate>
                                            <div class ="left" style="margin-left:100px">
									            <div class ="right">Ngày TBQHTN</div>
								            </div>
								            <div class="left width-100">
									            <eoscrm:TextBox ID="txtNgayTBQHTN" runat="server" tag="datevn" />
								            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                <%--</div>--%>
                                
                                <%--<div id="containerTNCN" style="display:block" runat="server">--%>
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
                                <%--</div>--%>
                                
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
	 <asp:UpdatePanel ID="upnlReport" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
			<div class="crmcontainer" id="divCR" runat="server">
                <%--table--%>
                
			</div>            
		</ContentTemplate>
	</asp:UpdatePanel>
     <asp:UpdatePanel ID="UptPanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="crmcontainer" id="contNhacNo" runat="server" visible="false">  
                <button id="btnNhacNoExcel" onclick="pheDuyetTNCN()">Duyệt tạm ngừng cấp nước</button> 
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

                        <asp:TemplateField HeaderText="" HeaderStyle-Width="80px">
                            <ItemTemplate>
                               <a id="huyPheDuyetBtn-<%# Eval("Idkh") %>" href="#" onclick="huyPheDuyetTNCN('<%# Eval("Idkh") %>')"><%# Eval("LabelHuyPheDuyet") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="" HeaderStyle-Width="80px">
                            <ItemTemplate>
                               <a id="boHuyPheDuyetBtn-<%# Eval("Idkh") %>" href="#" onclick="boHuyPheDuyetTNCN('<%# Eval("Idkh") %>')"><%# Eval("LabelBoHuyPheDuyet") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--<asp:TemplateField HeaderText="Duyệt TNCN" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <a href="javascript:void(0)" onclick="getTBTNCN(this)" idkh="<%#Eval("Idkh")%>" tenkh="<%#Eval("TenKH")%>" nam="<%#Eval("Nam")%>"
                                    ky="<%#Eval("Ky")%>" m3="<%#Eval("M3TinhTien")%>" ngaynhap="<%#Eval("NgayNhapCS")%>" tongtien="<%#Eval("TongTien")%>"
                                    sodienthoai="<%#Eval("SoDienThoai")%>" diachi="<%#Eval("DiaChi")%>" xncn="<%#Eval("XNCN")%>" ngaynhapstr="<%#Eval("NgayNhapCSStr")%>"
                                id="<%#Eval("Idkh")%>-<%#Eval("Nam")%>-<%#Eval("Ky")%>">Duyệt TNCN</a>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="pdf-viewer">
       <iframe id="pdf-frame" width="100%" height="100%"></iframe>
    </div>
    <div class="row" id="pdf-container" data-plugin="matchHeight" data-by-row="true" style="display: none;">
        <div class="container" id="pdf-advanced">
        </div>
    </div>
    <div id="loginvnpt">
       <input class="form-control" id="userName-vnpt" type="text" style="border: 1px solid black; width:100%; margin-bottom:20px" placeholder="Tài khoản VNPT Ca" value="046060004338"/>
       <input class="form-control" id="password-vnpt" type="password" style="border: 1px solid black; width:100%; margin-bottom:20px" placeholder="Mật khẩu VNPT Ca" value="Bom753159."/>
       <button class="form-control" onclick="loginVnptHash()" id="submitVnpt" style="margin-bottom:20px; width:100%">Đăng nhập</button>
    </div>
    <script src="../../content/scripts/exportExel.js"></script>
    <script>
        $(document).ready(function(){
            const uiDialogs = $(".ui-dialog")
            for(let i=0; i<uiDialogs.length; i++){
                if($(uiDialogs[i]).attr("aria-describedby")=="divLoading"){
                    $(uiDialogs[i]).css("left", $(document).width()/2-$(uiDialogs[i]).width()/2)
                    $(uiDialogs[i]).css("top", $(document).height()/2-$(uiDialogs[i]).height()/2)
                    break
                }
            }
        })
        
        
        const loginName = '<%=loginName%>'
        let khConNoObj;
        let base64PDf;
        let access_token;
        let listIdKHTNCN = [];
        let listIdBoHuyTNCN = [];
        let listIdKHHuyTNCN = [];

        function onSelectedCheckBox(idkh, m3TieuThu, tongTien, checkBox, ngayThanhToan, soDienThoai) {
            let obj = {
                idkh: idkh,
                m3TieuThu: m3TieuThu,
                tongTien: tongTien,
                ngayThanhToan: ngayThanhToan,
                soDienThoai: soDienThoai
            }
            if ($(checkBox).is(":checked")) {
                listIdKHTNCN.push(idkh)
            } else {
                for (let i = 0; i < listIdKHTNCN.length; i++) {
                    if (obj.idkh == listIdKHTNCN[i]) {
                        listIdKHTNCN.splice(i, 1)
                    }
                }
            }
        }
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
        function callApiDuyetTNCN() {
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
                    ListIDKH: listIdKHTNCN,
                    isGetAll: $("#chkAllTop").is(":checked")
                }
            )
            return $.ajax({
                url: "/Forms/DongMoNuocOnline/Leader_DuyetTNCN.aspx/PheDuyetTNCN",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ dieuKienLocStr: data, loginName: loginName }),
                async: true,
            })
        }
        function pheDuyetTNCN() {
            let isCheckAll = $("#chkAllTop").is(":checked");
            if (isCheckAll) {
                openWaitingDialog();
                listIdKHTNCN = []
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
                callApiDuyetTNCN()
                    .done(function (res) {
                        showInfor("Phê duyệt tạm ngưng cấp nước thành công")
                        $("#ctl00_mainCPH_btnBaoCao").click()
                        listIdKHTNCN = []
                        closeWaitingDialog()
                    })
                .fail(function (e) {
                    showError("Phê duyệt tạm ngưng cấp nước không thành công")
                    closeWaitingDialog()
                    console.log(e)
                })
            } else {
                if (listIdKHTNCN.length > 0) {
                    openWaitingDialog();
                    callApiDuyetTNCN()
                    .done(function (res) {
                        showInfor("Phê duyệt tạm ngưng cấp nước thành công")
                        $("#ctl00_mainCPH_btnBaoCao").click()
                        //for (let i = 0; i < listIdKHNhacNo.length; i++) {
                        //    let idkh = listIdKHNhacNo[i];
                        //    $("#huyPheDuyetBtn-" + idkh).text("Hủy phê duyệt")
                        //}
                        listIdKHTNCN = []
                        closeWaitingDialog()
                    })
                    .fail(function (res) {
                        showError("Phê duyệt tạm ngưng cấp nước không thành công")
                        listIdKHTNCN = []
                        closeWaitingDialog()
                    })
                } else {
                    showWarning("Không có khách hàng nào được chọn")
                }
            }
        }
        function getTBTNCN(tncn) {
            let ngayNhap = $(tncn).attr("ngaynhapstr");
            ngayNhap = ngayNhap.split("/")[0] + "-" + ngayNhap.split("/")[1] + "-" + ngayNhap.split("/")[2]
            $("#loginvnpt").dialog("open");
            //openWaitingDialog();
            let khConNo = {
                Idkh: $(tncn).attr("idkh"),
                Ky: $(tncn).attr("ky"),
                Nam: $(tncn).attr("nam"),
                TenKH: $(tncn).attr("tenkh"),
                DiaChi: $(tncn).attr("diachi"),
                XNCN: $(tncn).attr("xncn"),
                NgayNhapCSStr: ngayNhap
            }
            khConNoObj = khConNo;
            //callApiTBTNCN(khConNo)
            //.done(function (res) {
            //    closeWaitingDialog();
            //    displayPDF(res.d)
            //    $("#pdf-viewer").dialog("open");
            //})
            //.fail(function (err) {
            //    closeWaitingDialog();
            //    showError("Lỗi duyệt TNCN")
            //})
        }
        function callApiTBTNCN(khConNo) {
            return $.ajax({
                url: "/Forms/DongMoNuocOnline/DuyetTNCN.aspx/GetThongBaoTNCN",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ khConNoStr: JSON.stringify(khConNo) }),
                async: true,
            })
        }
        function callApiHuyPheDuyetTNCN() {
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
                    ListIDKH: listIdKHHuyTNCN,
                    isGetAll: $("#chkAllTop").is(":checked")
                }
            )
            return $.ajax({
                url: "/Forms/DongMoNuocOnline/Leader_DuyetTNCN.aspx/HuyPheDuyetTNCN",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ dieuKienLocStr: data }),
                async: true,
            })
        }
        function huyPheDuyetTNCN(idkh) {
            listIdKHHuyTNCN.push(idkh)
            openWaitingDialog();
            callApiHuyPheDuyetTNCN()
            .done(function () {
                showInfor("Hủy phê duyệt tạm ngừng cấp nước thành công")
                $("#ctl00_mainCPH_btnBaoCao").click()
                closeWaitingDialog();
                listIdKHHuyTNCN = []
            })
            .fail(function () {
                showError("Hủy phê duyệt tạm ngừng cấp nước không thành công")
                listIdKHHuyTNCN = []
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
                    listIdKHHuyTNCN = []
                    for (let i = 0; i < json.length; i++) {
                        listIdKHHuyTNCN.push(json[i]["IDKH"].trim())
                    }
                    openWaitingDialog();
                    callApiHuyPheDuyetTNCN()
                    .done(function () {
                        showInfor("Hủy phê duyệt thông báo quá hạn lần 2 thành công")
                        $("#ctl00_mainCPH_btnBaoCao").click()

                        listIdKHHuyTNCN = []
                    })
                    .fail(function () {
                        showError("Hủy phê duyệt không thành công")
                        listIdKHHuyTNCN = []
                        closeWaitingDialog();
                    })
                }
                reader.readAsArrayBuffer(file);
            }
        }
        function callApiBoHuyPheDuyetTNCN() {
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
                    ListIDKH: listIdBoHuyTNCN,
                    isGetAll: $("#chkAllTop").is(":checked")
                }
            )
            return $.ajax({
                url: "/Forms/DongMoNuocOnline/Leader_DuyetTNCN.aspx/BoHuyPheDuyetTNCN",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ dieuKienLocStr: data }),
                async: true,
            })
        }
        
        
        function boHuyPheDuyetTNCN(idkh) {
            listIdBoHuyTNCN.push(idkh)
            openWaitingDialog();
            callApiBoHuyPheDuyetTNCN()
            .done(function () {
                showInfor("Bỏ hủy phê duyệt tạm ngừng cấp nước thành công")
                $("#ctl00_mainCPH_btnBaoCao").click()
                closeWaitingDialog();
                listIdBoHuyTNCN = []
            })
            .fail(function () {
                showError("Bỏ hủy phê duyệt tạm ngừng cấp nước không thành công")
                listIdBoHuyTNCN = []
            })
        }
        
        function loginVnptHash() {
            let obj = {
                UserName: $("#userName-vnpt").val(),
                Password: $("#password-vnpt").val()
            }
            openWaitingDialog();
            $("#loginvnpt").dialog("close");
            $.ajax({
                url: "/Forms/DongMoNuocOnline/DuyetTNCN.aspx/LoginVnpt",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ loginEntityStr: JSON.stringify(obj) }),
                async: true,
            })
                .done(function (res) {
                    access_token = res.d.token
                    callApiTBTNCN(khConNoObj)
                        .done(function (res) {
                            closeWaitingDialog();
                            //displayPDF(res.d);
                            //$("#pdf-viewer").dialog("open");
                            let options = {
                                // Hàm callback khi chọn 'Đồng ý ký'
                                Callback: sign,
                                // Hàm callback khi chọn 'Không đồng ý ký'
                                onReject: null,
                                // Hàm callback khi tạo mới một mẫu chữ ký
                                onCreateSignatureImg: function (evt) {
                                    console.log(evt);
                                },
                                // Hàm callback khi xóa một mẫu chữ ký
                                onRemoveSignatureImg: function (evt) {
                                    console.log(evt);
                                },
                                // Danh sách mẫu chữ ký
                                signatureImgs: []
                            };
                            base64PDf = res.d
                            showPdfKy(res.d, options);

                        })
                        .fail(function (res) {
                            closeWaitingDialog();
                            showError("Không thể xuất thông báo")
                        })
                })
                .fail(function (err) {
                    showError("Sai tài khoản hoặc mật khẩu")
                    closeWaitingDialog();
                })
        }
        function sign(data) {
            if (!data.imageSrc) {
                alert("Vui lòng chọn chữ ký!");
                return;
            }
            let objDocSignStr = JSON.stringify({
                base64Doc: base64PDf,
                access_token: access_token,
                img: data.imageSrc,
                signatures: data.signatures,
                tenVB: "TNCN" + "-" + khConNoObj.Idkh + "-" + khConNoObj.Nam + "-" + khConNoObj.Ky
            })
            let objKHConNo = JSON.stringify({
                Nam: khConNoObj.Nam,
                Ky: khConNoObj.Ky,
                Idkh: khConNoObj.Idkh
            })
            openWaitingDialog()
            $.ajax({
                url: "/Forms/DongMoNuocOnline/DuyetTNCN.aspx/Sign",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: JSON.stringify({ docSignStr: objDocSignStr, khConNoStr: objKHConNo, loginName: loginName }),
                async: true,
            })
                .done(function (res) {
                    let idDuyet = khConNoObj.Idkh + "-" + khConNoObj.Nam + "-" + khConNoObj.Ky
                    $("#" + idDuyet).remove()
                    $("#pdf-container").dialog("close")
                    closeWaitingDialog()
            })
                .fail(function (err) {
                    showError("Lỗi ký số.Vui lòng ký lại")
                    closeWaitingDialog()
            })
        }
        function showPdfKy(base64, options) {

            $("#pdf-container").css("display", "block");
            let vnptPdf = $("#pdf-advanced").pdfsignature(options);
            //self.vnptPdf.initDataFile(file);
            vnptPdf.initDataBase64(base64);
            vnptPdf.start();
            $("#pdf-container").dialog("open");
        }
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
        $("#pdf-viewer").dialog({
            autoOpen: false,
            modal: true,
            width: $(document).width() / 2,
            height: $(document).height() / 2,
            title: "Thông báo tạm ngừng cấp nước"
        });
        $("#loginvnpt").dialog({
            autoOpen: false,
            modal: true,
            width: $(document).width() / 5,
            height: $(document).height() / 5,
            title: "Đăng nhập VNPT CA"
        });
        $("#pdf-container").dialog({
            autoOpen: false,
            modal: true,
            width: $(document).width(),
            height: $(document).height(),
            title: "Ký số"
        });
    </script>
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
            window.history.pushState({}, '', '?download=TBQHTN');
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
            //window.history.pushState({}, '', '?id=' + valSelected);
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