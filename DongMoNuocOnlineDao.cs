using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EOSCRM.Domain;
using EOSCRM.Util;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using itextSharpPDFText = iTextSharp.text;
using itextSharpPDF = iTextSharp.text.pdf;
using System.Data;
using System.Net;
using RestSharp;
using System.Threading;
using Newtonsoft.Json;

namespace EOSCRM.Dao
{

    public class DongMoNuocOnlineDao:BaseDao
    {
        public readonly string TBTN = "TBTT";
        public readonly string TBNN_1 = "TBNN_1";
        public readonly string TBQH_1 = "TBQH_1";
        public readonly string TBQH_2 = "TBQH_2";
        public readonly string TBTNCN = "TBTNCN";
        public readonly int totalPage = 1000;
        public readonly string rootFolder = "E:\\EbillingDongMoNuoc\\SourceCode\\EOSCRM.Web\\ThongBaos";
        public readonly string domainAPI = "http://192.168.0.15:6547/";//server LIVE
        //public readonly string domainAPI = "http://192.168.0.19:6547/";//server TEST
        //public readonly string domainAPI = "https://localhost:44311/";

        #region Giai đoạn thông báo tiền nước
        public List<KhConNo> GetKH_TB_TienNuoc(DieuKienLoc dieuKienLoc)
        {
            var IsGuiApp = false;
            if (dieuKienLoc.IsGuiAppCSKH != null || dieuKienLoc.IsGuiZalo != null)
            {
                IsGuiApp = true;
            }
            try
            {
                var listLastResult = new List<KhConNo>();
                var ListResult = _db.ExecuteQuery<KhConNo>(@"
                                                 SELECT
                                                        tt.IDKH as Idkh,
			                                            tt.TENKH as TenKH,
			                                            tt.NAM as Nam,
			                                            tt.THANG as Ky,
			                                            tt.M3TINHTIEN as M3TinhTien,
			                                            tt.NGAYNHAP as NgayNhapCS,
                                                        tt.TONGTIEN as TongTien,
                                                        kh.DIDONG1 as SoDienThoai,
			                                            kh.SONHA+', '+dp.TENDP+', '+kv.TENKV as DiaChi
                                                    FROM TIEUTHU as tt 
                                                    LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
                                                    LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
                                                    LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP" +
                                                    (IsGuiApp ? " LEFT JOIN DongMoNuocOnline_Message as dmnom ON dmnom.IDKH = tt.IDKH AND dmnom.THANG = tt.THANG AND dmnom.NAM = tt.NAM" : "") +
                                                        @" WHERE tt.HETNO = 0 
	                                                AND tt.TONGTIEN > 0
	                                                AND tt.NAM = " + dieuKienLoc.NamHd
                                                        + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                        + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                        + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                        + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
                                                        + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                        + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == null ? "" : dieuKienLoc.IsGuiAppCSKH == "true" ? " AND dmnom.IsGuiThongBaoTienNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTN + "'" + " AND dmnom.IsGuiAppCSKH = 1 AND (dmnom.IsXoaApp = 0 OR dmnom.IsXoaApp IS NULL) " : dieuKienLoc.IsGuiAppCSKH == "false" ? (@"
	                                                                                                                                                                                                                                                                                                                                                    AND (
                                                                                                                                                                                                                                                                                                                                                        (
		                                                                                                                                                                                                                                                                                                                                                    dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                                                                                    AND dmnom.LoaiThongBao = 'TBTT'
		                                                                                                                                                                                                                                                                                                                                                    AND (
				                                                                                                                                                                                                                                                                                                                                                    (dmnom.IsGuiAppCSKH = 0 OR dmnom.IsGuiAppCSKH IS NULL)
				                                                                                                                                                                                                                                                                                                                                                    OR dmnom.IsXoaApp = 1
			                                                                                                                                                                                                                                                                                                                                                    )
	                                                                                                                                                                                                                                                                                                                                                    )
	                                                                                                                                                                                                                                                                                                                                                    OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                                                                                    (
		                                                                                                                                                                                                                                                                                                                                                    SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                                                                                    WHERE IDKH = tt.IDKH 
		                                                                                                                                                                                                                                                                                                                                                    AND THANG = tt.THANG
		                                                                                                                                                                                                                                                                                                                                                    AND NAM = tt.NAM
		                                                                                                                                                                                                                                                                                                                                                    AND LoaiThongBao = 'TBTT'
                                                                                                                                                                                                                                                                                                                                                            AND IsGuiAppCSKH = 1 
                                                                                                                                                                                                                                                                                                                                                            AND IsXoaApp = 0
	                                                                                                                                                                                                                                                                                                                                                    ) IS NULL)
                                                                                                                                                                                                                                                                                                                                                    )") : "")
                                                        + (dieuKienLoc.IsGuiZalo == null ? "" : dieuKienLoc.IsGuiZalo == "true" ? " AND dmnom.IsGuiThongBaoTienNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTN + "'" + " AND dmnom.IsGuiZalo = 1" : dieuKienLoc.IsGuiZalo == "false" ? (@"
	                                                                                                                                                                                                                                                                                        AND (
                                                                                                                                                                                                                                                                                            (
		                                                                                                                                                                                                                                                                                        dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                        AND dmnom.LoaiThongBao = 'TBTT'
		                                                                                                                                                                                                                                                                                        AND (
				                                                                                                                                                                                                                                                                                        (dmnom.IsGuiZalo = 0 OR dmnom.IsGuiZalo IS NULL)
			                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                        OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                        (
		                                                                                                                                                                                                                                                                                        SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                        WHERE IDKH = tt.IDKH 
		                                                                                                                                                                                                                                                                                        AND THANG = tt.THANG
		                                                                                                                                                                                                                                                                                        AND NAM = tt.NAM
		                                                                                                                                                                                                                                                                                        AND LoaiThongBao = 'TBTT'
                                                                                                                                                                                                                                                                                                AND IsGuiZalo = 1
	                                                                                                                                                                                                                                                                                        ) IS NULL)
                                                                                                                                                                                                                                                                                        )") : "")
                                                        + " AND DATEDIFF(day, tt.NGAYNHAP, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayThongBaoTienNuoc)
                                                        + " ORDER BY tt.NGAYNHAPCS DESC"
                                                        )
                                            .Select(x => new KhConNo
                                            {
                                                Idkh = x.Idkh,
                                                TenKH = x.TenKH,
                                                Nam = x.Nam,
                                                Ky = x.Ky,
                                                M3TinhTien = x.M3TinhTien,
                                                NgayNhapCS = x.NgayNhapCS,
                                                NgayNhapCSStr = x.NgayNhapCS.ToString("dd/MM/yyyy"),
                                                NgayThongBaoNhacNoStr = x.NgayNhapCS.AddDays((double)ThongTinQuyTrinh.HanThanhToanTienNuoc).ToString("dd/MM/yyyy"),
                                                DiaChi = x.DiaChi,
                                                TongTien = x.TongTien,
                                                SoDienThoai = x.SoDienThoai
                                            })
                                            .ToList();
                
                foreach(var khConNo in ListResult){
                    if (listLastResult.Where(x => x.Idkh == khConNo.Idkh).FirstOrDefault()==null)
                    {
                        listLastResult.Add(khConNo);
                    }
                }
                return listLastResult;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        #endregion

        #region Giai đoạn nhắc nợ
        public List<KhConNo> GetKH_TB_NhacNo(DieuKienLoc dieuKienLoc)
        {
            var IsGuiApp = false;
            if (dieuKienLoc.IsGuiAppCSKH != null || dieuKienLoc.IsGuiZalo != null)
            {
                IsGuiApp = true;
            }
            var ListResult = new List<KhConNo>(); 
            var listLastResult = new List<KhConNo>(); 
            try
            {
                    ListResult = _db.ExecuteQuery<KhConNo>(@"
                                                 SELECT
                                                        tt.IDKH as Idkh,
			                                            tt.TENKH as TenKH,
			                                            tt.NAM as Nam,
			                                            tt.THANG as Ky,
			                                            tt.M3TINHTIEN as M3TinhTien,
			                                            tt.NGAYNHAP as NgayNhapCS,
                                                        tt.TONGTIEN as TongTien,
                                                        kh.DIDONG1 as SoDienThoai,
			                                            kh.SONHA+', '+dp.TENDP+', '+kv.TENKV as DiaChi
                                                    FROM TIEUTHU as tt 
                                                    LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
                                                    LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
                                                    LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP" +
                                                    (IsGuiApp ? " LEFT JOIN DongMoNuocOnline_Message as dmnom ON dmnom.IDKH = tt.IDKH AND dmnom.THANG = tt.THANG AND dmnom.NAM = tt.NAM" : "") +
                                                        @" WHERE tt.HETNO = 0 
	                                                AND tt.TONGTIEN > 0
	                                                AND tt.NAM = " + dieuKienLoc.NamHd
                                                        + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                        + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                        + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                        + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
                                                        + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                        + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == null ? "" : dieuKienLoc.IsGuiAppCSKH == "true" ? " AND dmnom.IsGuiThongBaoNhacNo = 1 AND dmnom.LoaiThongBao = " + "'" + TBNN_1 + "'" + " AND dmnom.IsGuiAppCSKH = 1 AND (dmnom.IsXoaApp = 0 OR dmnom.IsXoaApp IS NULL)" : dieuKienLoc.IsGuiAppCSKH == "false" ? (@"
	                                                                                                                                                                                                                                                                                                                                                    AND (
                                                                                                                                                                                                                                                                                                                                                        (
		                                                                                                                                                                                                                                                                                                                                                    dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                                                                                    AND dmnom.LoaiThongBao = 'TBNN_1'
		                                                                                                                                                                                                                                                                                                                                                    AND (
				                                                                                                                                                                                                                                                                                                                                                    (dmnom.IsGuiAppCSKH = 0 OR dmnom.IsGuiAppCSKH IS NULL)
				                                                                                                                                                                                                                                                                                                                                                    OR dmnom.IsXoaApp = 1
			                                                                                                                                                                                                                                                                                                                                                    )
	                                                                                                                                                                                                                                                                                                                                                    )
	                                                                                                                                                                                                                                                                                                                                                    OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                                                                                    (
		                                                                                                                                                                                                                                                                                                                                                    SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                                                                                    WHERE IDKH = tt.IDKH 
		                                                                                                                                                                                                                                                                                                                                                    AND THANG = tt.THANG
		                                                                                                                                                                                                                                                                                                                                                    AND NAM = tt.NAM
		                                                                                                                                                                                                                                                                                                                                                    AND LoaiThongBao = 'TBNN_1'
                                                                                                                                                                                                                                                                                                                                                            AND IsGuiAppCSKH = 1 
                                                                                                                                                                                                                                                                                                                                                            AND IsXoaApp = 0
	                                                                                                                                                                                                                                                                                                                                                    ) IS NULL)
                                                                                                                                                                                                                                                                                                                                                    )") : "")
                                                        + (dieuKienLoc.IsGuiZalo == null ? "" : dieuKienLoc.IsGuiZalo == "true" ? " AND dmnom.IsGuiThongBaoNhacNo = 1 AND dmnom.LoaiThongBao = " + "'" + TBNN_1 + "'" + " AND dmnom.IsGuiZalo = 1" : dieuKienLoc.IsGuiZalo == "false" ? (@"
	                                                                                                                                                                                                                                                                                        AND (
                                                                                                                                                                                                                                                                                            (
		                                                                                                                                                                                                                                                                                        dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                        AND dmnom.LoaiThongBao = 'TBNN_1'
		                                                                                                                                                                                                                                                                                        AND (
				                                                                                                                                                                                                                                                                                        (dmnom.IsGuiZalo = 0 OR dmnom.IsGuiZalo IS NULL)
			                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                        OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                        (
		                                                                                                                                                                                                                                                                                        SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                        WHERE IDKH = tt.IDKH 
		                                                                                                                                                                                                                                                                                        AND THANG = tt.THANG
		                                                                                                                                                                                                                                                                                        AND NAM = tt.NAM
		                                                                                                                                                                                                                                                                                        AND LoaiThongBao = 'TBNN_1'
                                                                                                                                                                                                                                                                                                AND IsGuiZalo = 1
	                                                                                                                                                                                                                                                                                        ) IS NULL)
                                                                                                                                                                                                                                                                                        )") : "")
                                                        + " AND DATEDIFF(day, tt.NGAYNHAP, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayThongBaoNhacNo)
                                                        + " ORDER BY tt.NGAYNHAPCS DESC"
                                                        )
                                            .Select(x => new KhConNo
                                            {
                                                Idkh = x.Idkh,
                                                TenKH = x.TenKH,
                                                Nam = x.Nam,
                                                Ky = x.Ky,
                                                M3TinhTien = x.M3TinhTien,
                                                NgayNhapCS = x.NgayNhapCS,
                                                NgayNhapCSStr = x.NgayNhapCS.ToString("dd/MM/yyyy"),
                                                NgayThongBaoNhacNoStr = x.NgayNhapCS.AddDays((double)ThongTinQuyTrinh.HanThanhToanNhacNo).ToString("dd/MM/yyyy"),
                                                DiaChi = x.DiaChi,
                                                TongTien = x.TongTien,
                                                SoDienThoai = x.SoDienThoai
                                            })
                                            .ToList();
                foreach (var khConNo in ListResult)
                {
                    if (listLastResult.Where(x => x.Idkh == khConNo.Idkh).FirstOrDefault() == null)
                    {
                        listLastResult.Add(khConNo);
                    }
                }

                return listLastResult;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        #endregion

        #region Giai đoạn thông báo quá hạn lần 1
            public List<KhConNo> GetKH_TB_QuaHan_1(DieuKienLoc dieuKienLoc)
        {
            var IsGuiApp = false;
            if (dieuKienLoc.IsGuiAppCSKH != null || dieuKienLoc.IsGuiZalo != null)
            {
                IsGuiApp = true;
            }
            try
            {
                var listLastResult = new List<KhConNo>();
                var ListResult = _db.ExecuteQuery<KhConNo>(@"
                                                 SELECT
                                                        tt.IDKH as Idkh,
			                                            tt.TENKH as TenKH,
			                                            tt.NAM as Nam,
			                                            tt.THANG as Ky,
			                                            tt.M3TINHTIEN as M3TinhTien,
			                                            tt.NGAYNHAP as NgayNhapCS,
                                                        tt.TONGTIEN as TongTien,
                                                        kh.DIDONG1 as SoDienThoai,
			                                            kh.SONHA+', '+dp.TENDP+', '+kv.TENKV as DiaChi
                                                    FROM TIEUTHU as tt"+
                                                    (IsGuiApp ? " LEFT JOIN DongMoNuocOnline_Message as dmnom ON dmnom.IDKH = tt.IDKH AND dmnom.THANG = tt.THANG AND dmnom.NAM = tt.NAM" : "")+
                                                    @" LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
                                                    LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
                                                    LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP" +
                                                        @" WHERE tt.HETNO = 0 
	                                                AND tt.TONGTIEN > 0
	                                                AND tt.NAM = " + dieuKienLoc.NamHd
                                                        + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                        + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                        + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                        + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
                                                        + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                        + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == null ? "" : dieuKienLoc.IsGuiAppCSKH == "true" ? " AND dmnom.IsGuiThongBaoQuaHanLan1 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_1 + "'" + " AND dmnom.IsGuiAppCSKH = 1 AND (dmnom.IsXoaApp = 0 OR dmnom.IsXoaApp IS NULL)" : dieuKienLoc.IsGuiAppCSKH == "false" ? (@"
	                                                                                                                                                                                                                                                                                                                                                        AND (
                                                                                                                                                                                                                                                                                                                                                            (
		                                                                                                                                                                                                                                                                                                                                                        dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                                                                                        AND dmnom.LoaiThongBao = 'TBQH_1'
		                                                                                                                                                                                                                                                                                                                                                        AND (
				                                                                                                                                                                                                                                                                                                                                                        (dmnom.IsGuiAppCSKH = 0 OR dmnom.IsGuiAppCSKH IS NULL)
				                                                                                                                                                                                                                                                                                                                                                        OR dmnom.IsXoaApp = 1
			                                                                                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                                                                                        OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                                                                                        (
		                                                                                                                                                                                                                                                                                                                                                        SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                                                                                        WHERE IDKH = tt.IDKH 
		                                                                                                                                                                                                                                                                                                                                                        AND THANG = tt.THANG
		                                                                                                                                                                                                                                                                                                                                                        AND NAM = tt.NAM
		                                                                                                                                                                                                                                                                                                                                                        AND LoaiThongBao = 'TBQH_1'
                                                                                                                                                                                                                                                                                                                                                                AND IsGuiAppCSKH = 1 
                                                                                                                                                                                                                                                                                                                                                                AND IsXoaApp = 0
	                                                                                                                                                                                                                                                                                                                                                        ) IS NULL)
                                                                                                                                                                                                                                                                                                                                                        )") : "")
                                                        + (dieuKienLoc.IsGuiZalo == null ? "" : dieuKienLoc.IsGuiZalo == "true" ? " AND dmnom.IsGuiThongBaoQuaHanLan1 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_1 + "'" + " AND dmnom.IsGuiZalo = 1" : dieuKienLoc.IsGuiZalo == "false" ? (@"
	                                                                                                                                                                                                                                                                                         AND (
                                                                                                                                                                                                                                                                                            (
		                                                                                                                                                                                                                                                                                        dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                        AND dmnom.LoaiThongBao = 'TBQH_1'
		                                                                                                                                                                                                                                                                                        AND (
				                                                                                                                                                                                                                                                                                        (dmnom.IsGuiZalo = 0 OR dmnom.IsGuiZalo IS NULL)
			                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                        OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                        (
		                                                                                                                                                                                                                                                                                        SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                        WHERE IDKH = tt.IDKH 
		                                                                                                                                                                                                                                                                                        AND THANG = tt.THANG
		                                                                                                                                                                                                                                                                                        AND NAM = tt.NAM
		                                                                                                                                                                                                                                                                                        AND LoaiThongBao = 'TBQH_1'
                                                                                                                                                                                                                                                                                                AND IsGuiZalo = 1
	                                                                                                                                                                                                                                                                                        ) IS NULL)
                                                                                                                                                                                                                                                                                        )") : "")
                                                        + " AND DATEDIFF(day, tt.NGAYNHAP, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTBQHTT_1)
                                                        + " ORDER BY tt.NGAYNHAPCS DESC"
                                                        )
                                            .Select(x => new KhConNo
                                            {
                                                Idkh = x.Idkh,
                                                TenKH = x.TenKH,
                                                Nam = x.Nam,
                                                Ky = x.Ky,
                                                M3TinhTien = x.M3TinhTien,
                                                NgayNhapCS = x.NgayNhapCS,
                                                NgayNhapCSStr = x.NgayNhapCS.ToString("dd/MM/yyyy"),
                                                NgayThongBaoNhacNoStr = x.NgayNhapCS.AddDays((double)ThongTinQuyTrinh.HanThanhToanQH_1).ToString("dd/MM/yyyy"),
                                                DiaChi = x.DiaChi,
                                                TongTien = x.TongTien,
                                                SoDienThoai = x.SoDienThoai
                                            })
                                            .ToList();

                foreach (var khConNo in ListResult)
                {
                    if (listLastResult.Where(x => x.Idkh == khConNo.Idkh).FirstOrDefault() == null)
                    {
                        listLastResult.Add(khConNo);
                    }
                }
                return listLastResult;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        #endregion

        #region Giai đoạn thông báo quá hạn lần 2
            public List<KhConNo> GetKH_TB_QuaHan_2_To_Leader_PheDuyet(DieuKienLoc dieuKienLoc)
            {
                try
                {

                    var ListResult = _db.ExecuteQuery<KhConNo>(@"
                                                     SELECT
                                                            tt.IDKH as Idkh,
			                                                tt.TENKH as TenKH,
			                                                tt.NAM as Nam,
			                                                tt.THANG as Ky,
			                                                tt.M3TINHTIEN as M3TinhTien,
			                                                tt.NGAYNHAP as NgayNhapCS,
                                                            tt.TONGTIEN as TongTien,
                                                            kh.DIDONG1 as SoDienThoai,
                                                            dmno.IsDeletedTBQH2 as IsDeletedTBQH2,
			                                                kh.SONHA+', '+dp.TENDP+', '+kv.TENKV as DiaChi,
                                                            dmno.LeaderDuyetTBQH2 as LeaderPheDuyetTBQH2
                                                        FROM TIEUTHU as tt 
                                                        LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
                                                        LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
                                                        LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP
                                                        LEFT JOIN DongMoNuocOnline as dmno ON tt.IDKH = dmno.IDKH AND tt.THANG = dmno.THANG AND tt.NAM = dmno.NAM" +
                                                            @" WHERE tt.HETNO = 0 
	                                                    AND tt.TONGTIEN > 0
                                                        AND dmno.ManagerDuyetTBQH2 IS NULL
                                                        AND PathThongBao_TBQHTN_2 IS NOT NULL
	                                                    AND tt.NAM = " + dieuKienLoc.NamHd
                                                            + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                            + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                            + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                            + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
                                                            + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                            + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                            + " AND DATEDIFF(day, tt.NGAYNHAP, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTBQHTT_2)
                                                            + " ORDER BY tt.NGAYNHAPCS DESC"
                                                            )
                                                .Select(x => new KhConNo
                                                {
                                                    Idkh = x.Idkh,
                                                    TenKH = x.TenKH,
                                                    Nam = x.Nam,
                                                    Ky = x.Ky,
                                                    M3TinhTien = x.M3TinhTien,
                                                    NgayNhapCS = x.NgayNhapCS,
                                                    NgayNhapCSStr = x.NgayNhapCS.ToString("dd/MM/yyyy"),
                                                    NgayThongBaoNhacNoStr = x.NgayNhapCS.AddDays((double)ThongTinQuyTrinh.HanThanhToanQH_2).ToString("dd/MM/yyyy"),
                                                    DiaChi = x.DiaChi,
                                                    TongTien = x.TongTien,
                                                    SoDienThoai = x.SoDienThoai,
                                                    IsDeletedTBQH2= x.IsDeletedTBQH2,
                                                    LeaderPheDuyetTBQH2 = x.LeaderPheDuyetTBQH2 != null ? "ĐÃ TRÌNH" : "CHƯA TRÌNH", 
                                                    ManagerDuyetTBQH2 = x.ManagerDuyetTBQH2 != null ? "GDXN ĐÃ PHÊ DUYỆT" : "GDXN CHƯA PHÊ DUYỆT",
                                                    LabelHuyPheDuyet = x.IsDeletedTBQH2 == null || x.IsDeletedTBQH2 == false ? "Hủy phê duyệt" : "",
                                                    LabelBoHuyPheDuyet = x.IsDeletedTBQH2 != null && x.IsDeletedTBQH2 == true ? "Bỏ hủy phê duyệt" :""
                                                })
                                                .ToList();


                    return ListResult;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
            public List<KhConNo> GetKH_TB_QuaHan_2_To_Manager_PheDuyet(DieuKienLoc dieuKienLoc)
            {
                try
                {

                    var ListResult = _db.ExecuteQuery<KhConNo>(@"
                                                     SELECT
                                                            tt.IDKH as Idkh,
			                                                tt.TENKH as TenKH,
			                                                tt.NAM as Nam,
			                                                tt.THANG as Ky,
			                                                tt.M3TINHTIEN as M3TinhTien,
			                                                tt.NGAYNHAP as NgayNhapCS,
                                                            tt.TONGTIEN as TongTien,
                                                            kh.DIDONG1 as SoDienThoai,
                                                            dmno.IsDeletedTBQH2 as IsDeletedTBQH2,
			                                                kh.SONHA+', '+dp.TENDP+', '+kv.TENKV as DiaChi,
                                                            dmno.ManagerDuyetTBQH2 as ManagerDuyetTBQH2,
                                                            dmno.LeaderDuyetTBQH2 as LeaderDuyetTBQH2
                                                        FROM TIEUTHU as tt 
                                                        LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
                                                        LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
                                                        LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP
                                                        LEFT JOIN DongMoNuocOnline as dmno ON tt.IDKH = dmno.IDKH AND tt.THANG = dmno.THANG AND tt.NAM = dmno.NAM" +
                                                            @" WHERE tt.HETNO = 0 
	                                                    AND tt.TONGTIEN > 0
                                                        AND dmno.LeaderDuyetTBQH2 IS NOT NULL
                                                        AND PathThongBao_TBQHTN_2 IS NOT NULL
	                                                    AND tt.NAM = " + dieuKienLoc.NamHd
                                                            + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                            + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                            + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                            + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
                                                            + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                            + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                            + " AND DATEDIFF(day, tt.NGAYNHAP, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTBQHTT_2)
                                                            + " ORDER BY tt.NGAYNHAPCS DESC"
                                                            )
                                                .Select(x => new KhConNo
                                                {
                                                    Idkh = x.Idkh,
                                                    TenKH = x.TenKH,
                                                    Nam = x.Nam,
                                                    Ky = x.Ky,
                                                    M3TinhTien = x.M3TinhTien,
                                                    NgayNhapCS = x.NgayNhapCS,
                                                    NgayNhapCSStr = x.NgayNhapCS.ToString("dd/MM/yyyy"),
                                                    NgayThongBaoNhacNoStr = x.NgayNhapCS.AddDays((double)ThongTinQuyTrinh.HanThanhToanQH_2).ToString("dd/MM/yyyy"),
                                                    DiaChi = x.DiaChi,
                                                    TongTien = x.TongTien,
                                                    SoDienThoai = x.SoDienThoai,
                                                    IsDeletedTBQH2 = x.IsDeletedTBQH2,
                                                    LeaderPheDuyetTBQH2 = x.LeaderPheDuyetTBQH2 != null ? "ĐÃ TRÌNH" : "CHƯA TRÌNH",
                                                    ManagerDuyetTBQH2 = x.ManagerDuyetTBQH2 != null ? "GDXN ĐÃ PHÊ DUYỆT" : "GDXN CHƯA PHÊ DUYỆT",
                                                    LabelHuyPheDuyet = x.IsDeletedTBQH2==null || x.IsDeletedTBQH2==false ? "Hủy phê duyệt" :""
                                                })
                                                .ToList();


                    return ListResult;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
            public int LeaderPheDuyetThongBaoQuaHanLan2(DieuKienLoc dieuKienLoc, string nguoiPheDuyet)
            {
                var listIdkh = dieuKienLoc.ListIDKH;
                if (!dieuKienLoc.isGetAll)
                {
                    if (listIdkh.Count() > 0)
                    {
                        for (var i = 0; i < listIdkh.Count(); i++)
                        {
                            try
                            {
                                //                        _db.ExecuteCommand(String.Format(@"INSERT INTO DongMoNuocOnline(NAM, THANG, IDKH, STATUS_DMNO, NV_Duyet_TBQHTN_1, NGAY_TBQHTN_2)
                                //                                                        SELECT '{0}', '{1}', '{2}', '{3}', '{4}', '{5}'
                                //                                                        WHERE NOT EXISTS(
                                //                                                            SELECT 1 FROM DongMoNuocOnline WHERE NAM = '{0}' AND THANG = '{1}' AND IDKH = '{2}'
                                //                                                        )", dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i], TBQH_1, nguoiPheDuyet, DateTime.Now.ToString("yyyy-MM-dd")));
                                //                        _db.ExecuteCommand(String.Format(@"MERGE INTO [DongMoNuocOnline] AS target
                                //                                                            USING (SELECT '{0}' AS NAM, '{1}' AS THANG, '{2}' AS IDKH) AS source
                                //                                                            ON (target.NAM = source.NAM AND target.THANG = source.THANG AND target.IDKH = source.IDKH)
                                //                                                            WHEN MATCHED THEN UPDATE SET target.STATUS_DMNO= '{3}', target.IsDeletedTBQH2 = NULL, target.LeaderDuyetTBQH2 = '{4}'
                                //                                                            WHEN NOT MATCHED THEN INSERT (NAM, THANG, IDKH, STATUS_DMNO, LeaderDuyetTBQH2, NGAY_TBQHTN_2) 
                                //                                                            VALUES (source.NAM, source.THANG, source.IDKH, '{3}', '{4}', '{5}');",
                                //                                                             dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i], TBQH_2, nguoiPheDuyet, DateTime.Now.ToString("yyyy-MM-dd")));
                                _db.ExecuteCommand(String.Format(@"UPDATE [DongMoNuocOnline] SET STATUS_DMNO = '{0}', IsDeletedTBQH2 = {1}, LeaderDuyetTBQH2='{2}' 
                                                               WHERE NAM = {3} AND THANG = {4} AND IDKH = '{5}' AND ManagerDuyetTBQH2 IS NULL AND IsDeletedTBQH2 IS NULL
                                                               AND PathThongBao_TBQHTN_2 IS NOT NULL
                                                               AND DATEDIFF(day, NgayNhapCS, '{6}') = {7}",
                                                               TBQH_2, "NULL", nguoiPheDuyet, dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i], dieuKienLoc.NgayLoc, ThongTinQuyTrinh.NgayTBQHTT_2));

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    try
                    {
                        _db.ExecuteCommand(String.Format(@"UPDATE [DongMoNuocOnline] SET STATUS_DMNO = '{0}', IsDeletedTBQH2 = {1}, LeaderDuyetTBQH2='{2}' 
                                                               WHERE NAM = {3} AND THANG = {4} AND XNCN = N'{5}' AND ManagerDuyetTBQH2 IS NULL AND IsDeletedTBQH2 IS NULL
                                                               AND PathThongBao_TBQHTN_2 IS NOT NULL AND DATEDIFF(day, NgayNhapCS, '{6}') = {7}",
                                                                  TBQH_2, "NULL", nguoiPheDuyet, dieuKienLoc.NamHd, dieuKienLoc.KyHd, dieuKienLoc.XNCN, dieuKienLoc.NgayLoc, ThongTinQuyTrinh.NgayTBQHTT_2));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
               
                }
            
                return 200;
            }
            public int ManagerPheDuyetThongBaoQuaHanLan2(DieuKienLoc dieuKienLoc, string nguoiPheDuyet)
            {
                var listIdkh = dieuKienLoc.ListIDKH;
                if (!dieuKienLoc.isGetAll)
                {
                    if (listIdkh.Count() > 0)
                    {
                        for (var i = 0; i < listIdkh.Count(); i++)
                        {
                            try
                            {
                                //                        _db.ExecuteCommand(String.Format(@"INSERT INTO DongMoNuocOnline(NAM, THANG, IDKH, STATUS_DMNO, NV_Duyet_TBQHTN_1, NGAY_TBQHTN_2)
                                //                                                        SELECT '{0}', '{1}', '{2}', '{3}', '{4}', '{5}'
                                //                                                        WHERE NOT EXISTS(
                                //                                                            SELECT 1 FROM DongMoNuocOnline WHERE NAM = '{0}' AND THANG = '{1}' AND IDKH = '{2}'
                                //                                                        )", dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i], TBQH_1, nguoiPheDuyet, DateTime.Now.ToString("yyyy-MM-dd")));
                                //                        _db.ExecuteCommand(String.Format(@"MERGE INTO [DongMoNuocOnline] AS target
                                //                                                            USING (SELECT '{0}' AS NAM, '{1}' AS THANG, '{2}' AS IDKH) AS source
                                //                                                            ON (target.NAM = source.NAM AND target.THANG = source.THANG AND target.IDKH = source.IDKH)
                                //                                                            WHEN MATCHED THEN UPDATE SET target.STATUS_DMNO= '{3}', target.IsDeletedTBQH2 = NULL, target.ManagerDuyetTBQH2 = '{4}'
                                //                                                            WHEN NOT MATCHED THEN INSERT (NAM, THANG, IDKH, STATUS_DMNO, ManagerDuyetTBQH2, NGAY_TBQHTN_2) 
                                //                                                            VALUES (source.NAM, source.THANG, source.IDKH, '{3}', '{4}', '{5}');",
                                //                                                             dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i], TBQH_2, nguoiPheDuyet, DateTime.Now.ToString("yyyy-MM-dd")));
                                _db.ExecuteCommand(String.Format(@"UPDATE [DongMoNuocOnline] SET STATUS_DMNO = '{0}', IsDeletedTBQH2 = {1}, ManagerDuyetTBQH2='{2}' 
                                                               WHERE NAM = {3} AND THANG = {4} AND IDKH = '{5}' AND LeaderDuyetTBQH2 IS NOT NULL
                                                                AND PathThongBao_TBQHTN_2 IS NOT NULL
                                                                AND DATEDIFF(day, NgayNhapCS, '{6}') = {7}",
                                                                   TBQH_2, "NULL", nguoiPheDuyet, dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i], dieuKienLoc.NgayLoc, ThongTinQuyTrinh.NgayTBQHTT_2));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    try
                    {
                        _db.ExecuteCommand(String.Format(@"UPDATE [DongMoNuocOnline] SET STATUS_DMNO = '{0}', IsDeletedTBQH2 = {1}, ManagerDuyetTBQH2='{2}'
                                                        WHERE NAM = {3} AND THANG = {4} AND XNCN = N'{5}' AND LeaderDuyetTBQH2 IS NOT NULL
                                                        AND DATEDIFF(day, NgayNhapCS, '{6}') = {7}",
                                                           TBQH_2, "NULL", nguoiPheDuyet, dieuKienLoc.NamHd, dieuKienLoc.KyHd, dieuKienLoc.XNCN, dieuKienLoc.NgayLoc, ThongTinQuyTrinh.NgayTBQHTT_2));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            
                return 200;
            }
            public List<KhConNo> GetKH_TB_QuaHan_2_DaPheDuyet(DieuKienLoc dieuKienLoc) {
                //WHERE tt.HETNO = 0 
                //AND tt.TONGTIEN > 0
                var IsGuiApp = false;
                if (dieuKienLoc.IsGuiAppCSKH != null || dieuKienLoc.IsGuiZalo != null)
                {
                    IsGuiApp = true;
                }
                try
                {
                    var listKhConNo = new List<KhConNo>();
                    if (!IsGuiApp)
                    {
                        listKhConNo = _db.ExecuteQuery<KhConNo>(@"
                                                SELECT
                                                    tt.IDKH as Idkh,
	                                                tt.TENKH as TenKH,
	                                                tt.NAM as Nam,
	                                                tt.THANG as Ky,
	                                                tt.M3TINHTIEN as M3TinhTien,
	                                                tt.NGAYNHAP as NgayNhapCS,
                                                    tt.TONGTIEN as TongTien,
                                                    kh.DIDONG1 as SoDienThoai,
	                                                kh.SONHA+', '+dp.TENDP+', '+kv.TENKV as DiaChi,
                                                    kv.TENHIEU as XNCN
                                                FROM TIEUTHU as tt" +
                                                " LEFT JOIN DongMoNuocOnline as dmno ON dmno.IDKH = tt.IDKH AND dmno.NAM = tt.NAM AND dmno.THANG = tt.THANG"
                                                + (IsGuiApp ? @" LEFT JOIN DongMoNuocOnline_Message as dmnom ON dmnom.IDKH = tt.IDKH AND dmnom.NAM = tt.NAM AND dmnom.THANG = tt.THANG" : "") +
                                                @" LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
                                                LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
                                                LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP
                                                WHERE tt.HETNO = 0 
                                                AND tt.TONGTIEN > 0
                                                AND dmno.ManagerDuyetTBQH2 IS NOT NULL
                                                AND tt.NAM = " + dieuKienLoc.NamHd
                                                    + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                    + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                    + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                    + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
                                                    + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                    + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                    + (dieuKienLoc.IsGuiAppCSKH == null ? "" : dieuKienLoc.IsGuiAppCSKH == "true" ? " AND dmnom.IsGuiThongBaoQuaHanLan2 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_2 + "'" + " AND dmnom.IsGuiAppCSKH = 1 AND (dmnom.IsXoaApp = 0 OR dmnom.IsXoaApp IS NULL)" : dieuKienLoc.IsGuiAppCSKH == "false" ? " AND (dmnom.IsGuiThongBaoQuaHanLan2 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_2 + "' AND ((dmnom.IsGuiAppCSKH = 0 OR dmnom.IsGuiAppCSKH IS NULL) OR (dmnom.IsXoaApp = 1)))" : "")
                                                    + (dieuKienLoc.IsGuiZalo == null ? "" : dieuKienLoc.IsGuiZalo == "true" ? " AND dmnom.IsGuiThongBaoQuaHanLan2 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_2 + "'" + " AND dmnom.IsGuiZalo = 1" : dieuKienLoc.IsGuiZalo == "false" ? " AND dmnom.IsGuiThongBaoQuaHanLan2 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_2 + "' AND (dmnom.IsGuiZalo = 0 OR dmnom.IsGuiZalo IS NULL)" : "")
                                                    + " AND DATEDIFF(day, tt.NGAYNHAP, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTBQHTT_2)
                                                    + " ORDER BY tt.NGAYNHAP DESC")
                                                    .Select(x => new KhConNo
                                                    {
                                                        Idkh = x.Idkh,
                                                        TenKH = x.TenKH,
                                                        Nam = x.Nam,
                                                        Ky = x.Ky,
                                                        M3TinhTien = x.M3TinhTien,
                                                        NgayNhapCS = x.NgayNhapCS,
                                                        NgayNhapCSStr = x.NgayNhapCS.ToString("dd/MM/yyyy"),
                                                        NgayThongBaoNhacNoStr = x.NgayNhapCS.AddDays((double)ThongTinQuyTrinh.HanThanhToanQH_2).ToString("dd/MM/yyyy"),
                                                        DiaChi = x.DiaChi,
                                                        TongTien = x.TongTien,
                                                        SoDienThoai = x.SoDienThoai,
                                                        XNCN = x.XNCN
                                                    })
                                                    .ToList();
                    }
                    else
                    {
                        var listKhConNoLastResult = new List<KhConNo>();
                        listKhConNo = _db.ExecuteQuery<KhConNo>(@"SELECT a.* FROM (SELECT
		                                                        tt.IDKH as Idkh,
		                                                        tt.TENKH as TenKH,
		                                                        tt.NAM as Nam,
		                                                        tt.THANG as Ky,
		                                                        tt.M3TINHTIEN as M3TinhTien,
		                                                        tt.NGAYNHAPCS as NgayNhapCS,
		                                                        tt.TONGTIEN as TongTien,
		                                                        kh.DIDONG1 as SoDienThoai,
		                                                        kh.SONHA+', '+dp.TENDP+', '+kv.TENKV as DiaChi,
		                                                        kv.TENHIEU as XNCN,
                                                                tt.HETNO,
		                                                        dmno.STATUS_DMNO
	                                                        FROM TIEUTHU as tt
	                                                        LEFT JOIN KHACHHANG as kh ON tt.IDKH = kh.IDKH
	                                                        LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
	                                                        LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP
	                                                        LEFT JOIN DongMoNuocOnline as dmno ON dmno.IDKH = tt.IDKH AND dmno.NAM = tt.NAM AND dmno.THANG = tt.THANG
	                                                        WHERE tt.HETNO = 0 
	                                                        AND tt.TONGTIEN > 0
	                                                        AND dmno.ManagerDuyetTBQH2 IS NOT NULL
                                                            AND tt.NAM = " + dieuKienLoc.NamHd
                                                            + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                            + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                            + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                            + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
                                                            + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                            + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                            + " AND dmno.STATUS_DMNO = " + "'" + TBQH_2 + "'"
                                                            + " AND DATEDIFF(day, tt.NGAYNHAP, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTBQHTT_2)
                                                        +@" ) as a LEFT JOIN DongMoNuocOnline_Message as dmnom
                                                        ON a.Idkh = dmnom.IDKH AND a.Ky = dmnom.THANG AND a.Nam = dmnom.NAM AND a.STATUS_DMNO = dmnom.LoaiThongBao
                                                        WHERE a.HETNO = 0"
                                                        + (dieuKienLoc.IsGuiAppCSKH == null ? "" : dieuKienLoc.IsGuiAppCSKH == "true" ? " AND dmnom.IsGuiThongBaoQuaHanLan2 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_2 + "'" + " AND dmnom.IsGuiAppCSKH = 1 AND (dmnom.IsXoaApp = 0 OR dmnom.IsXoaApp IS NULL)" : dieuKienLoc.IsGuiAppCSKH == "false" ? (@"
	                                                                                                                                                                                                                                                                                                                                                         AND (
                                                                                                                                                                                                                                                                                                                                                            (
		                                                                                                                                                                                                                                                                                                                                                        dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                                                                                        AND dmnom.LoaiThongBao = 'TBQH_2'
		                                                                                                                                                                                                                                                                                                                                                        AND (
				                                                                                                                                                                                                                                                                                                                                                        (dmnom.IsGuiAppCSKH = 0 OR dmnom.IsGuiAppCSKH IS NULL)
				                                                                                                                                                                                                                                                                                                                                                        OR dmnom.IsXoaApp = 1
			                                                                                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                                                                                        OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                                                                                        (
		                                                                                                                                                                                                                                                                                                                                                        SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                                                                                        WHERE IDKH = a.IDKH 
		                                                                                                                                                                                                                                                                                                                                                        AND THANG = a.Ky
		                                                                                                                                                                                                                                                                                                                                                        AND NAM = a.NAM
		                                                                                                                                                                                                                                                                                                                                                        AND LoaiThongBao = 'TBQH_2'
                                                                                                                                                                                                                                                                                                                                                                AND IsGuiAppCSKH = 1 
                                                                                                                                                                                                                                                                                                                                                                AND IsXoaApp = 0
	                                                                                                                                                                                                                                                                                                                                                        ) IS NULL)
                                                                                                                                                                                                                                                                                                                                                        )") : "")
                                                        + (dieuKienLoc.IsGuiZalo == null ? "" : dieuKienLoc.IsGuiZalo == "true" ? " AND dmnom.IsGuiThongBaoQuaHanLan2 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_2 + "'" + " AND dmnom.IsGuiZalo = 1" : dieuKienLoc.IsGuiZalo == "false" ? (@"
	                                                                                                                                                                                                                                                                                         AND (
                                                                                                                                                                                                                                                                                            (
		                                                                                                                                                                                                                                                                                        dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                        AND dmnom.LoaiThongBao = 'TBQH_2'
		                                                                                                                                                                                                                                                                                        AND (
				                                                                                                                                                                                                                                                                                        (dmnom.IsGuiZalo = 0 OR dmnom.IsGuiZalo IS NULL)
			                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                        OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                        (
		                                                                                                                                                                                                                                                                                        SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                        WHERE IDKH = a.IDKH 
		                                                                                                                                                                                                                                                                                        AND THANG = a.Ky
		                                                                                                                                                                                                                                                                                        AND NAM = a.NAM
		                                                                                                                                                                                                                                                                                        AND LoaiThongBao = 'TBQH_2'
                                                                                                                                                                                                                                                                                                AND IsGuiZalo = 1
	                                                                                                                                                                                                                                                                                        ) IS NULL)
                                                                                                                                                                                                                                                                                        )") : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == null && dieuKienLoc.IsGuiZalo == "false" ? " OR dmnom.LoaiThongBao IS NULL" : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == "false" && dieuKienLoc.IsGuiZalo == null ? " OR dmnom.LoaiThongBao IS NULL" : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == "false" && dieuKienLoc.IsGuiZalo == "false" ? " OR dmnom.LoaiThongBao IS NULL" : "")
                                                        )
                                                        .Select(x => new KhConNo
                                                        {
                                                            Idkh = x.Idkh,
                                                            TenKH = x.TenKH,
                                                            Nam = x.Nam,
                                                            Ky = x.Ky,
                                                            M3TinhTien = x.M3TinhTien,
                                                            NgayNhapCS = x.NgayNhapCS,
                                                            NgayNhapCSStr = x.NgayNhapCS.ToString("dd/MM/yyyy"),
                                                            NgayThongBaoNhacNoStr = x.NgayNhapCS.AddDays((double)ThongTinQuyTrinh.HanThanhToanQH_2).ToString("dd/MM/yyyy"),
                                                            DiaChi = x.DiaChi,
                                                            TongTien = x.TongTien,
                                                            SoDienThoai = x.SoDienThoai,
                                                            XNCN = x.XNCN
                                                        })
                                                        .ToList();
                        foreach (var khConNo in listKhConNo)
                        {
                            if (listKhConNoLastResult.Where(x => x.Idkh == khConNo.Idkh).FirstOrDefault() == null)
                            {
                                listKhConNoLastResult.Add(khConNo);
                            }
                        }
                        return listKhConNoLastResult;
                    }
                    return listKhConNo;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
           
            }
            public KhConNo Count_TB_QuaHan_DaPheDuyet(DieuKienLoc dieuKienLoc)
            {
                var IsGuiApp = false;
                if (dieuKienLoc.IsGuiAppCSKH != null || dieuKienLoc.IsGuiZalo != null)
                {
                    IsGuiApp = true;
                }
                var countQuaHanPheDuyet = 0;
                try
                {
                    if (!IsGuiApp)
                    {
                        countQuaHanPheDuyet = _db.ExecuteQuery<int>(@"SELECT
                                                    COUNT(tt.IDKH)
                                                FROM DongMoNuocOnline as dmno
                                                JOIN TIEUTHU as tt ON dmno.IDKH = tt.IDKH AND dmno.NAM = tt.NAM AND dmno.THANG = tt.THANG
                                                LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH"
                                                + (IsGuiApp ? @" LEFT JOIN DongMoNuocOnline_Message as dmnom ON dmnom.IDKH = tt.IDKH AND dmnom.NAM = tt.NAM AND dmnom.THANG = tt.THANG" : "") +
                                                @" LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
                                                LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP
                                                WHERE dmno.NGAY_TBQHTN_2 IS NOT NULL
                                                AND dmno.NV_Duyet_TBQHTN_2 IS NOT NULL
                                                AND (dmno.IsDeletedTBQH2 <> 1 OR dmno.IsDeletedTBQH2 IS NULL)
                                                AND tt.NAM = " + dieuKienLoc.NamHd
                                                    + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                    + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                    + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                    + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
                                                    + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                    + (dieuKienLoc.IsGuiAppCSKH == null ? "" : dieuKienLoc.IsGuiAppCSKH == "true" ? " AND dmnom.IsGuiThongBaoQuaHanLan2 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_2 + "'" + " AND dmnom.IsGuiAppCSKH = 1 AND (dmnom.IsXoaApp = 0 OR dmnom.IsXoaApp IS NULL)" : dieuKienLoc.IsGuiAppCSKH == "false" ? " AND dmnom.IsGuiThongBaoQuaHanLan2 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_2 + "' AND ((dmnom.IsGuiAppCSKH = 0 OR dmnom.IsGuiAppCSKH IS NULL) OR (dmnom.IsXoaApp = 1))" : "")
                                                    + (dieuKienLoc.IsGuiZalo == null ? "" : dieuKienLoc.IsGuiZalo == "true" ? " AND dmnom.IsGuiThongBaoQuaHanLan2 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_2 + "'" + " AND dmnom.IsGuiZalo = 1" : dieuKienLoc.IsGuiZalo == "false" ? " AND dmnom.IsGuiThongBaoQuaHanLan2 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_2 + "' AND (dmnom.IsGuiZalo = 0 OR dmnom.IsGuiZalo IS NULL)" : "")
                                                    + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                    + " AND DATEDIFF(day, tt.NGAYNHAP, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTBQHTT_2)
                                                    + (dieuKienLoc.isZaloAndApp == "false" ? " AND ((kh.ZALOUSERID IS NULL OR kh.ZALOUSERID = '') AND kh.isZNS = 0)"
                                                        : dieuKienLoc.isZaloAndApp == "true" ? " AND ((kh.ZALOUSERID IS NOT NULL AND kh.ZALOUSERID <> '') OR kh.isZNS = 1)" : ""))
                                                    .FirstOrDefault();
                    }
                    else
                    {
                        countQuaHanPheDuyet = _db.ExecuteQuery<int>(@"SELECT COUNT(a.Idkh) FROM (SELECT
		                                                        tt.IDKH as Idkh,
		                                                        tt.TENKH as TenKH,
		                                                        tt.NAM as Nam,
		                                                        tt.THANG as Ky,
		                                                        tt.M3TINHTIEN as M3TinhTien,
		                                                        tt.NGAYNHAPCS as NgayNhapCS,
		                                                        tt.TONGTIEN as TongTien,
		                                                        kh.DIDONG1 as SoDienThoai,
		                                                        kh.SONHA+', '+dp.TENDP+', '+kv.TENKV as DiaChi,
		                                                        kv.TENHIEU as XNCN,
                                                                tt.HETNO,
		                                                        dmno.STATUS_DMNO
	                                                        FROM TIEUTHU as tt
	                                                        LEFT JOIN KHACHHANG as kh ON tt.IDKH = kh.IDKH
	                                                        LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
	                                                        LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP
	                                                        LEFT JOIN DongMoNuocOnline as dmno ON dmno.IDKH = tt.IDKH AND dmno.NAM = tt.NAM AND dmno.THANG = tt.THANG
	                                                        WHERE tt.HETNO = 0 
	                                                        AND tt.TONGTIEN > 0
	                                                        AND dmno.ManagerDuyetTBQH2 IS NOT NULL
                                                            AND tt.NAM = " + dieuKienLoc.NamHd
                                                            + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                            + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                            + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                            + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
                                                            + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                            + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                            + " AND dmno.STATUS_DMNO = " + "'" + TBQH_2 + "'"
                                                            + " AND DATEDIFF(day, tt.NGAYNHAPCS, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTBQHTT_2)
                                                        + @" ) as a LEFT JOIN DongMoNuocOnline_Message as dmnom
                                                        ON a.Idkh = dmnom.IDKH AND a.Ky = dmnom.THANG AND a.Nam = dmnom.NAM AND a.STATUS_DMNO = dmnom.LoaiThongBao
                                                        WHERE a.HETNO = 0"
                                                        + (dieuKienLoc.IsGuiAppCSKH == null ? "" : dieuKienLoc.IsGuiAppCSKH == "true" ? " AND dmnom.IsGuiThongBaoQuaHanLan2 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_2 + "'" + " AND dmnom.IsGuiAppCSKH = 1 AND (dmnom.IsXoaApp = 0 OR dmnom.IsXoaApp IS NULL)" : dieuKienLoc.IsGuiAppCSKH == "false" ? (@"
	                                                                                                                                                                                                                                                                                                                                                         AND (
                                                                                                                                                                                                                                                                                                                                                            (
		                                                                                                                                                                                                                                                                                                                                                        dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                                                                                        AND dmnom.LoaiThongBao = 'TBQH_2'
		                                                                                                                                                                                                                                                                                                                                                        AND (
				                                                                                                                                                                                                                                                                                                                                                        (dmnom.IsGuiAppCSKH = 0 OR dmnom.IsGuiAppCSKH IS NULL)
				                                                                                                                                                                                                                                                                                                                                                        OR dmnom.IsXoaApp = 1
			                                                                                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                                                                                        OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                                                                                        (
		                                                                                                                                                                                                                                                                                                                                                        SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                                                                                        WHERE IDKH = a.IDKH 
		                                                                                                                                                                                                                                                                                                                                                        AND THANG = a.Ky
		                                                                                                                                                                                                                                                                                                                                                        AND NAM = a.NAM
		                                                                                                                                                                                                                                                                                                                                                        AND LoaiThongBao = 'TBQH_2'
                                                                                                                                                                                                                                                                                                                                                                AND IsGuiAppCSKH = 1 
                                                                                                                                                                                                                                                                                                                                                                AND IsXoaApp = 0
	                                                                                                                                                                                                                                                                                                                                                        ) IS NULL)
                                                                                                                                                                                                                                                                                                                                                        )") : "")
                                                        + (dieuKienLoc.IsGuiZalo == null ? "" : dieuKienLoc.IsGuiZalo == "true" ? " AND dmnom.IsGuiThongBaoQuaHanLan2 = 1 AND dmnom.LoaiThongBao = " + "'" + TBQH_2 + "'" + " AND dmnom.IsGuiZalo = 1" : dieuKienLoc.IsGuiZalo == "false" ? (@"
	                                                                                                                                                                                                                                                                                         AND (
                                                                                                                                                                                                                                                                                            (
		                                                                                                                                                                                                                                                                                        dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                        AND dmnom.LoaiThongBao = 'TBQH_2'
		                                                                                                                                                                                                                                                                                        AND (
				                                                                                                                                                                                                                                                                                        (dmnom.IsGuiZalo = 0 OR dmnom.IsGuiZalo IS NULL)
			                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                        OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                        (
		                                                                                                                                                                                                                                                                                        SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                        WHERE IDKH = a.IDKH 
		                                                                                                                                                                                                                                                                                        AND THANG = a.Ky
		                                                                                                                                                                                                                                                                                        AND NAM = a.NAM
		                                                                                                                                                                                                                                                                                        AND LoaiThongBao = 'TBQH_2'
                                                                                                                                                                                                                                                                                                AND IsGuiZalo = 1
	                                                                                                                                                                                                                                                                                        ) IS NULL)
                                                                                                                                                                                                                                                                                        )") : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == null && dieuKienLoc.IsGuiZalo == "false" ? " OR dmnom.LoaiThongBao IS NULL" : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == "false" && dieuKienLoc.IsGuiZalo == null ? " OR dmnom.LoaiThongBao IS NULL" : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == "false" && dieuKienLoc.IsGuiZalo == "false" ? " OR dmnom.LoaiThongBao IS NULL" : "")
                                                        )
                                                        .FirstOrDefault();
                    }
                    
                    var totalPages = Math.Ceiling((double)(countQuaHanPheDuyet / totalPage));

                    return new KhConNo { 
                        totalKh = countQuaHanPheDuyet,
                        totalPage = totalPages
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return new KhConNo();
            
            }
            //public string SetGiayThongBaoQuaHan2(KhConNo khConNo, GDXN gdxn)
            //{
            //    var fontSize12 = "20px";
            //    var fontSize13 = "21.66px";
            //    var fontSize14 = "23.34px";
            //    var lineSpace = "8px";

            //    khConNo.NgayNhapCSStr = String.Join("-", khConNo.NgayNhapCSStr.Split('-').Reverse());
            //    DateTime ngayCupNuoc = DateTime.Parse(khConNo.NgayNhapCSStr).AddDays(ThongTinQuyTrinh.HanThanhToanQH_2);

            //    int dateCupNuoc = ngayCupNuoc.Day;
            //    int monthCupNuoc = ngayCupNuoc.Month;
            //    int yearCupNuoc = ngayCupNuoc.Year;

            //    string kyHd = khConNo.Ky < 10 ? "0" + khConNo.Ky + "/" + khConNo.Nam : khConNo.Ky + "/" + khConNo.Nam;
            //    var htmlContent = String.Format(
            //            "<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />" +
            //                "<div style=\"color:white\">.</div>" +
            //                "<div style=\"margin-top:20px\">" +
            //                "<table style=\"width:100%; \">" +
            //                    "<tr>" +
            //                        "<td style=\"text-align:center\">" +
            //                           "<div style=\"font-size:{0};\">CÔNG TY CỔ PHẦN CẤP NƯỚC</div>" +
            //                           "<div style=\"font-size:{0};\">THỪA THIÊN HUẾ</div>" +
            //                           "<div style=\"font-size:{0}; font-weight:bold;\">CHI NHÁNH (XÍ NGHIỆP CẤP</div>" +
            //                           "<div style=\"font-size:{0}; font-weight:bold;\">NƯỚC {14})</div>" +
            //                           "<div style=\"color:white; border-bottom:1px solid black; width:100px; margin-left: 100px; margin-top: 10px\"></div>" +
            //                        "</td>" +
            //                        "<td style=\"text-align:center; vertical-align: top\">" +
            //                           "<div style=\"font-size:{0}; font-weight:bold\">CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</div>" +
            //                           "<div style=\"font-size:{0}; font-weight:bold; text-decoration: underline\">Độc lập - Tự do - Hạnh phúc</div>" +
            //                           "<br/>" +
            //                           "<div style=\"font-size:{0}; font-style:italic\">Thừa Thiên Huế, ngày {11} tháng {12} năm {13} </div>" +
            //                        "</td>" +
            //                    "</tr>" +
            //                "</table>" +
            //                "<br/>" +
            //                "<table style=\"width:100%;\">" +
            //                    "<tr>" +
            //                        "<td style=\"text-align:center\">" +
            //                            "<div style=\"font-size:{2}; font-weight:bold\">THÔNG BÁO</div>" +

            //                        "</td>" +
            //                    "</tr>" +
            //                    "<tr>" +
            //                        "<td style=\"text-align:center\">" +
            //                            "<div style=\"font-size:{2}; font-weight:bold;\">Về việc thanh toán tiền nước quá hạn lần 2</div>" +
            //                            "<div style=\"color:white; border-bottom:1px solid black; width:100px; margin-left: 380px; margin-top: 10px\"></div>" +
            //                        "</td>" +
            //                    "</tr>" +
            //                "</table>" +
            //                "<table style=\"margin-left:100px; margin-top:20px\">" +
            //                    "<tr style=\"margin-bottom:{3}; \">" +
            //                        "<td style=\"font-size:{1}; text-align:right\"><span style=\"font-weight:bold; text-decoration:underline\">Kính gửi</span>: Khách hàng:</td>" +
            //                        "<td style=\"width: 100px\"></td>" +
            //                        "<td style=\"font-size:{1}\">{7}</td>" +
            //                    "</tr>" +
            //                    "<tr style=\"margin-bottom:{3}; \">" +
            //                        "<td style=\"font-size:{1}; text-align:right\">Địa chỉ: </td>" +
            //                        "<td style=\"width: 100px\"></td>" +
            //                        "<td style=\"font-size:{1}\">{5}</td>" +
            //                    "</tr>" +
            //                    "<tr style=\"margin-bottom:{3}; \">" +
            //                        "<td style=\"font-size:{1}; text-align:right\">Mã khách hàng: </td>" +
            //                        "<td style=\"width: 100px\"></td>" +
            //                        "<td style=\"font-size:{1}\">{4}</td>" +
            //                    "</tr>" +
            //                "</table>" +
            //                "<br/>" +

            //                "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">Công ty Cổ phần Cấp Nước Thừa Thiên Huế cảm ơn Quý khách hàng đã sử dụng dịch</div>" +
            //                "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">vụ của Công ty và xin trân trọng thông báo: Đến nay, đã quá hạn nhưng quý khách hàng vẫn</div>" +
            //                "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">chưa thanh toán tiền nước kỳ {6}. Khối lượng sử dụng: {17} m<sup>3</sup>. Số tiền: {18} đồng.</div>" +

            //                "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">Khuyến khích khách hàng thanh toán không sử dụng tiền mặt thông qua tài khoản các</div>" +
            //                "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">ngân hàng, các ứng dụng thanh toán khác (ví điện tử,…)</div>" +

            //                "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">Hạn thanh toán: trước ngày {8} tháng {9} năm {10}</div>" +

            //                "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">Quá thời hạn trên mà Quý khách hàng vẫn chưa thanh toán, Công ty xin phép tạm</div>" +
            //                "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">ngừng dịch vụ cấp nước. Việc cấp nước trở lại được thực hiện sau 24 giờ kể từ khi khách</div>" +
            //                "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">hàng thực hiện thanh toán tiền nước và chi phí liên quan (vật tư, nhân công đóng mở nước</div>" +
            //                "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">theo quy định hiện hành và các chi phí phát sinh khác).</div>" +

            //                "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">Trường hợp Qúy khách hàng nhiều lần vi phạm nghĩa vụ thanh toán tiền nước, Công</div>" +
            //                "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">ty có thể kéo dài thời gian ngừng dịch vụ cấp nước hoặc đơn phương chấm dứt hợp đồng,</div>" +
            //                "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">cắt hủy đồng hồ đo đếm nước sạch; (trường hợp khách hàng yêu cầu cung cấp nước sạch</div>" +
            //                "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">lại phải chịu mọi chi phí phát sinh theo quy định bao gồm cả chi phí đồng hồ,…).</div>" +

            //                "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">Rất mong nhận được sự hợp tác của Quý khách hàng.</div>" +

            //                "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">Trung tâm CSKH (24/7, miễn phí cước) 1800 0036.</div>" +

            //                "<table style=\"width:100%\">" +
            //                    "<tr>" +
            //                        "<td style=\"text-align:right; font-size:{1}; font-weight:bold\">" +
            //                               "<p>GIÁM ĐỐC CHI NHÁNH<p>" +
            //                               "<div><img {19} style=\"margin-left: 570px;\" src=\"data:image/png;base64,{16}\"/></div>" +
            //                        "</td>" +
            //                    "</tr>" +
            //                "</table>" +
            //            "</div>"
            //        ,
            //        fontSize12,//0
            //        fontSize13,//1
            //        fontSize14,//2
            //        lineSpace,//3
            //        khConNo.Idkh,//4
            //        khConNo.DiaChi,//5
            //        kyHd,//6
            //        khConNo.TenKH,//7
            //        dateCupNuoc,//8
            //        monthCupNuoc,//9
            //        yearCupNuoc,//10
            //        DateTime.Now.Day,//11
            //        DateTime.Now.Month,//12
            //        DateTime.Now.Year,//13
            //        khConNo.XNCN.ToUpper(),//14
            //        khConNo.XNCN,//15
            //        gdxn.ChuKi,//16
            //        khConNo.M3TinhTien.ToString("N0").Replace(',', '.'),//17,
            //        khConNo.TongTien.ToString("N0").Replace(',', '.'),//18;
            //        gdxn.HeightChuKi!=0 && gdxn.WidthChuKi!=0 ? String.Format("width=\"{0}px\" height=\"{1}px\"", gdxn.WidthChuKi, gdxn.HeightChuKi):"");
            //    var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(htmlContent);
            //    return Convert.ToBase64String(pdfBytes);
            //}
//            public List<KhConNo> GetAllGiayThongBaoQuaHan2_V2(List<KhConNo> listKhConNo, DieuKienLoc dieuKienLoc)
//            {
//                try
//                {
//                    if (!dieuKienLoc.isGetAll)
//                    {
//                        var listStringIdkh = "";
//                        for (var i = 0; i < listKhConNo.Count(); i++)
//                        {
//                            if (i != listKhConNo.Count() - 1)
//                            {
//                                listStringIdkh += "'" + listKhConNo[i].Idkh + "'" + ",";
//                            }
//                            else
//                            {
//                                listStringIdkh += "'" + listKhConNo[i].Idkh + "'";
//                            }
//                        }
//                        var result = _db.ExecuteQuery<KhConNo>(String.Format(@"SELECT PathThongBao_TBQHTN_2 as PathThongBao FROM DongMoNuocOnline 
//                                                                                      WHERE THANG = {1} AND NAM = {2} AND IDKH IN ({0}) 
//                                                                                      AND (IsDeletedTBQH2 <> 1 OR IsDeletedTBQH2 IS NULL)
//                                                                                      AND ManagerDuyetTBQH2 IS NOT NULL",
//                                                                                listStringIdkh, listKhConNo[0].Ky, listKhConNo[0].Nam))
//                                                                                .Select(x=>new KhConNo{
//                                                                                    base64GiayTB = ConvertFileToBase64(rootFolder + x.PathThongBao)
//                                                                                })
//                                                                                .ToList();
                    
//                        return result;
//                    }else{
                        
//                            var listKhConNoo = _db.ExecuteQuery<KhConNo>(@"SELECT
//                                                    dmno.PathThongBao_TBQHTN_2 as PathThongBao
//                                                FROM DongMoNuocOnline as dmno
//                                                JOIN TIEUTHU as tt ON dmno.IDKH = tt.IDKH AND dmno.NAM = tt.NAM AND dmno.THANG = tt.THANG
//                                                JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
//                                                JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
//                                                JOIN DUONGPHO as dp ON dp.MADP = kh.MADP
//                                                AND dmno.NGAY_TBQHTN_2 IS NOT NULL
//                                                AND (dmno.IsDeletedTBQH2 <> 1 OR dmno.IsDeletedTBQH2 IS NULL)
//                                                AND ManagerDuyetTBQH2 IS NOT NULL
//                                                AND tt.NAM = " + dieuKienLoc.NamHd
//                                                + " AND tt.THANG = " + dieuKienLoc.KyHd
//                                                + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
//                                                + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
//                                                + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
//                                                + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
//                                                + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
//                                                + " AND DATEDIFF(day, tt.NGAYNHAPCS, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTBQHTT_2)
//                                                + (dieuKienLoc.isZaloAndApp == "false" ? " AND ((kh.ZALOUSERID IS NULL OR kh.ZALOUSERID = '') AND kh.isZNS = 0)"
//                                                    : dieuKienLoc.isZaloAndApp == "true" ? " AND ((kh.ZALOUSERID IS NOT NULL AND kh.ZALOUSERID <> '') OR kh.isZNS = 1)" : "")
//                                                + " ORDER BY tt.IDKH DESC "
//                                                + "OFFSET " + dieuKienLoc.Page + " ROWS FETCH NEXT " + totalPage + " ROWS ONLY ")
//                                                .Select(x => new KhConNo
//                                                {
//                                                    base64GiayTB = ConvertFileToBase64(rootFolder + x.PathThongBao)
//                                                })
//                                                .ToList();
//                            return listKhConNoo;
//                    }
               
//                }
//                catch (Exception e)
//                {
//                    Console.WriteLine(e);
//                }
//                return new List<KhConNo>();
           
//            }
            //public string GetAllGiayThongBaoQuaHan2_PDF(List<KhConNo> listKhConNo, DieuKienLoc dieuKienLoc)
            //{
            //    var listStr = new List<string>();


            //    //var gdxn = GetChuKiGDXN(listKhConNo[0].XNCN);
            //    //for (int i = 0; i < listKhConNo.Count(); i++)
            //    //{
            //    //    listStr.Add(SetGiayThongBaoQuaHan2(listKhConNo[i], gdxn));
            //    //}

            //    var listBase64 = GetAllGiayThongBaoQuaHan2_V2(listKhConNo, dieuKienLoc);
            //    for (int i = 0; i < listBase64.Count(); i++)
            //    {
            //        listStr.Add(listBase64[i].base64GiayTB);
            //    }
            //    return GhepBase64ToPDF(listStr);
            //}
            public string GetAllGiayThongBaoQuaHan2_PDF(List<KhConNo> listKhConNo, DieuKienLoc dieuKienLoc)
            {
                var client = new RestClient(domainAPI + "api/tb/tbqh2");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = new
                {
                    ListKHConNo = listKhConNo,
                    DieuKienLoc = dieuKienLoc
                };
                request.AddJsonBody(body);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<string>(response.Content); 
                }
                else
                {
                    Console.WriteLine(response.ErrorMessage);
                    return null;
                }
            }
            public int Leader_HuyPheDuyetTBQH2(DieuKienLoc dieuKienLoc)
            {
                var listIdkh = dieuKienLoc.ListIDKH;
                if (listIdkh.Count() > 0)
                {
                    for (var i = 0; i < listIdkh.Count(); i++)
                    {
                        try
                        {
                            _db.ExecuteCommand(String.Format(@"UPDATE [DongmoNuocOnline] SET IsDeletedTBQH2 = 1, LeaderDuyetTBQH2 = NULL WHERE NAM = {0} AND THANG = {1} AND IDKH = {2} AND ManagerDuyetTBQH2 IS NULL",
                                                               dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i]));
                        }
                        catch (Exception e)
                        {
                            throw new Exception();
                        }

                    }
                }
                else
                {
                    return 0;
                }
                return 200;
            }
            public int Leader_Bo_HuyPheDuyetTBQH2(DieuKienLoc dieuKienLoc)
            {
                var listIdkh = dieuKienLoc.ListIDKH;
                if (listIdkh.Count() > 0)
                {
                    for (var i = 0; i < listIdkh.Count(); i++)
                    {
                        try
                        {
                            _db.ExecuteCommand(String.Format(@"UPDATE [DongmoNuocOnline] SET IsDeletedTBQH2 = NULL, LeaderDuyetTBQH2 = NULL WHERE NAM = {0} AND THANG = {1} AND IDKH = {2} AND IsDeletedTBQH2 = 1",
                                                               dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i]));
                        }
                        catch (Exception e)
                        {
                            throw new Exception();
                        }

                    }
                }
                else
                {
                    return 0;
                }
                return 200;
            }
            public int Manager_HuyPheDuyetTBQH2(DieuKienLoc dieuKienLoc)
            {
                var listIdkh = dieuKienLoc.ListIDKH;
                //huy hang loat
                if (dieuKienLoc.isGetAll) {
                    try
                    {
                        _db.ExecuteCommand(String.Format(@"UPDATE [DongmoNuocOnline] SET ManagerDuyetTBQH2 = NULL, LeaderDuyetTBQH2 = NULL WHERE NAM = {0} AND THANG = {1} AND XNCN = N'{2}'",
                                                           dieuKienLoc.NamHd, dieuKienLoc.KyHd, dieuKienLoc.XNCN));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                //huy chi dinh tung khach hang
                else
                {
                    if (listIdkh.Count() > 0)
                    {
                        for (var i = 0; i < listIdkh.Count(); i++)
                        {
                            try
                            {
                                _db.ExecuteCommand(String.Format(@"UPDATE [DongmoNuocOnline] SET ManagerDuyetTBQH2 = NULL, LeaderDuyetTBQH2 = NULL WHERE NAM = {0} AND THANG = {1} AND IDKH = {2}",
                                                                   dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i]));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
            
                return 200;
            }
        #endregion

        #region Giai đoạn thông báo tạm ngừng cấp nước
            public List<KhConNo> GetKH_TNCN_To_Leader_PheDuyet(DieuKienLoc dieuKienLoc)
            {
                try
                {

                    var ListResult = _db.ExecuteQuery<KhConNo>(@"
                                                     SELECT
                                                            tt.IDKH as Idkh,
			                                                tt.TENKH as TenKH,
			                                                tt.NAM as Nam,
			                                                tt.THANG as Ky,
			                                                tt.M3TINHTIEN as M3TinhTien,
			                                                tt.NGAYNHAP as NgayNhapCS,
                                                            tt.TONGTIEN as TongTien,
                                                            kh.DIDONG1 as SoDienThoai,
			                                                kh.SONHA+', '+dp.TENDP+', '+kv.TENKV as DiaChi,
                                                            kv.TENHIEU as XNCN,
                                                            dmno.LeaderDuyetTNCN as LeaderPheDuyetTNCN,
                                                            dmno.IsDeletedTNCN as IsDeletedTNCN
                                                        FROM TIEUTHU as tt 
                                                        LEFT JOIN KHACHHANG as kh ON tt.IDKH = kh.IDKH
                                                        LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
                                                        LEFT JOIN DongMoNuocOnline as dmno ON dmno.IDKH = tt.IDKH AND dmno.NAM = tt.NAM AND dmno.THANG = tt.THANG
                                                        LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP" +
                                                            @" WHERE tt.HETNO = 0 
	                                                    AND tt.TONGTIEN > 0
	                                                    AND tt.NAM = " + dieuKienLoc.NamHd
                                                            + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                            + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                            + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                            + (dieuKienLoc.MaDuongPho != null ? String.Format(" AND dp.TENDP LIKE N'%{0}%'", dieuKienLoc.MaDuongPho) : "")
                                                            + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                            + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                            + " AND DATEDIFF(day, tt.NGAYNHAP, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTNCN)
                                                            + " AND dmno.PathThongBao_TNCN IS NOT NULL"
                                                            + " AND dmno.ManagerDuyetTNCN IS NULL"
                                                            + " ORDER BY tt.NGAYNHAPCS DESC"
                                                            )
                                                .Select(x => new KhConNo
                                                {
                                                    Idkh = x.Idkh,
                                                    TenKH = x.TenKH,
                                                    Nam = x.Nam,
                                                    Ky = x.Ky,
                                                    M3TinhTien = x.M3TinhTien,
                                                    NgayNhapCS = x.NgayNhapCS,
                                                    NgayNhapCSStr = x.NgayNhapCS.ToString("dd/MM/yyyy"),
                                                    NgayThongBaoNhacNoStr = x.NgayNhapCS.AddDays((double)ThongTinQuyTrinh.NgayTNCN).ToString("dd/MM/yyyy"),
                                                    DiaChi = x.DiaChi,
                                                    TongTien = x.TongTien,
                                                    SoDienThoai = x.SoDienThoai,
                                                    XNCN = x.XNCN,
                                                    LeaderPheDuyetTNCN = x.LeaderPheDuyetTNCN == null ? "CHƯA TRÌNH" : "ĐÃ TRÌNH",
                                                    LabelHuyPheDuyet = x.IsDeletedTNCN == null || x.IsDeletedTNCN == false ? "Hủy phê duyệt" : "",
                                                    LabelBoHuyPheDuyet = x.IsDeletedTNCN != null && x.IsDeletedTNCN == true ? "Bỏ hủy phê duyệt" : ""
                                                })
                                                .ToList();


                    return ListResult;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
            public List<KhConNo> GetKH_TNCN_To_Manager_PheDuyet(DieuKienLoc dieuKienLoc)
            {
                try
                {

                    var ListResult = _db.ExecuteQuery<KhConNo>(@"
                                                     SELECT
                                                            tt.IDKH as Idkh,
			                                                tt.TENKH as TenKH,
			                                                tt.NAM as Nam,
			                                                tt.THANG as Ky,
			                                                tt.M3TINHTIEN as M3TinhTien,
			                                                tt.NGAYNHAP as NgayNhapCS,
                                                            tt.TONGTIEN as TongTien,
                                                            kh.DIDONG1 as SoDienThoai,
                                                            dmno.IsDeletedTNCN as IsDeletedTNCN,
                                                            dmno.IsKySoTNCN as IsKySoTNCN,
			                                                kh.SONHA+', '+dp.TENDP+', '+kv.TENKV as DiaChi,
                                                            dmno.ManagerDuyetTNCN as ManagerDuyetTNCN,
                                                            dmno.PathThongBao_TNCN as PathThongBao_TNCN
                                                        FROM TIEUTHU as tt 
                                                        LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
                                                        LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
                                                        LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP
                                                        LEFT JOIN DongMoNuocOnline as dmno ON tt.IDKH = dmno.IDKH AND tt.THANG = dmno.THANG AND tt.NAM = dmno.NAM" +
                                                            @" WHERE tt.HETNO = 0 
	                                                    AND tt.TONGTIEN > 0
                                                        AND dmno.LeaderDuyetTNCN IS NOT NULL
                                                        AND dmno.PathThongBao_TNCN IS NOT NULL
	                                                    AND tt.NAM = " + dieuKienLoc.NamHd
                                                            + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                            + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                            + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                            + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
                                                            + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                            + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                            + " AND DATEDIFF(day, tt.NGAYNHAP, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTNCN)
                                                            + " ORDER BY tt.NGAYNHAPCS DESC"
                                                            )
                                                .Select(x => new KhConNo
                                                {
                                                    Idkh = x.Idkh,
                                                    TenKH = x.TenKH,
                                                    Nam = x.Nam,
                                                    Ky = x.Ky,
                                                    M3TinhTien = x.M3TinhTien,
                                                    NgayNhapCS = x.NgayNhapCS,
                                                    NgayNhapCSStr = x.NgayNhapCS.ToString("dd/MM/yyyy"),
                                                    NgayThongBaoNhacNoStr = x.NgayNhapCS.AddDays((double)ThongTinQuyTrinh.HanThanhToanQH_2).ToString("dd/MM/yyyy"),
                                                    DiaChi = x.DiaChi,
                                                    TongTien = x.TongTien,
                                                    SoDienThoai = x.SoDienThoai,
                                                    IsDeletedTNCN = x.IsDeletedTNCN,
                                                    ManagerDuyetTNCN = x.ManagerDuyetTNCN != null ? "GDXN ĐÃ PHÊ DUYỆT" : "GDXN CHƯA PHÊ DUYỆT",
                                                    LabelHuyPheDuyet = x.IsDeletedTNCN == null || x.IsDeletedTBQH2 == false ? "Hủy phê duyệt" : "",
                                                    LabelKySoTNCN = x.IsKySoTNCN == null || x.IsKySoTNCN == false ? "Ký số duyệt TNCN" : ""
                                                })
                                                .ToList();


                    return ListResult;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
            public string SetGiayThongBaoTNCN(KhConNo khConNo)
            {
                var fontSize12 = "20px";
                var fontSize13 = "21.66px";
                var fontSize14 = "23.34px";
                var lineSpace = "8px";

                khConNo.NgayNhapCSStr = String.Join("-", khConNo.NgayNhapCSStr.Split('-').Reverse());
                DateTime ngayCupNuoc = DateTime.Parse(khConNo.NgayNhapCSStr).AddDays(ThongTinQuyTrinh.NgayTNCN);

                int dateCupNuoc = ngayCupNuoc.Day;
                int monthCupNuoc = ngayCupNuoc.Month;
                int yearCupNuoc = ngayCupNuoc.Year;

                string kyHd = khConNo.Ky < 10 ? "0" + khConNo.Ky + "/" + khConNo.Nam : khConNo.Ky + "/" + khConNo.Nam;
                var htmlContent = String.Format(
                        "<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />" +
                            "<div style=\"color:white\">.</div>" +
                            "<div style=\"margin-top:20px\">" +
                            "<table style=\"width:100%; \">" +
                                "<tr>" +
                                    "<td style=\"text-align:center\">" +
                                       "<div style=\"font-size:{0};\">CÔNG TY CỔ PHẦN CẤP NƯỚC</div>" +
                                       "<div style=\"font-size:{0};\">THỪA THIÊN HUẾ</div>" +
                                       "<div style=\"font-size:{0}; font-weight:bold; text-decoration: underline\">XÍ NGHIỆP CẤP NƯỚC {14}</div>" +
                                       "<div style=\"font-size:{0};\">Số : ……/TB-XN…</div>" +
                                    "</td>" +
                                    "<td style=\"text-align:center; vertical-align: top\">" +
                                       "<div style=\"font-size:{0}; font-weight:bold\">CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</div>" +
                                       "<div style=\"font-size:{0}; font-weight:bold; text-decoration: underline\">Độc lập - Tự do - Hạnh phúc</div>" +
                                       "<br/>" +
                                       "<div style=\"font-size:{0}; font-style:italic\">Thừa Thiên Huế, ngày {11} tháng {12} năm {13} </div>" +
                                    "</td>" +
                                "</tr>" +
                            "</table>" +
                            "<br/>" +
                            "<table style=\"width:100%;\">" +
                                "<tr>" +
                                    "<td style=\"text-align:center\">" +
                                        "<div style=\"font-size:{2}; font-weight:bold\">THÔNG BÁO</div>" +

                                    "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td style=\"text-align:center\">" +
                                        "<div style=\"font-size:{2}; font-weight:bold;\">V/v tạm ngừng dịch vụ cấp nước</div>" +
                                        "<div style=\"color:white; border-bottom:1px solid black; width:100px; margin-left: 380px; margin-top: 10px\"></div>" +
                                    "</td>" +
                                "</tr>" +

                                "<tr>" +
                                    "<td style=\"text-align:center\">" +
                                        "<div style=\"font-size:{1}; margin-bottom:{3}\">Kính gửi: {7}</div>" +
                                    "</td>" +
                                "</tr>" +
                            "</table>" +
                            "<div style=\"margin-left:120px\">" +
                                "<div style=\"font-size:{1}; margin-bottom:{3}\">- Mã số khách hàng: {4}</div>" +
                                "<div style=\"font-size:{1}; margin-bottom:{3}\">- Địa chỉ: {5}</div>" +
                                "<div style=\"font-size:{1}; margin-bottom:{3}\">- Địa chỉ sử dụng nước: {5}</div>" +
                                "<div style=\"font-style: italic; font-size:{1}; margin-bottom:{3}\">Căn cứ Điều 5 của Hợp đồng dịch vụ cấp nước sạch đã ký kết giữa Khách hàng và </div>" +
                            "</div>" +
                            "<div style=\"font-style: italic; margin-left:58px; font-size:{1}; margin-bottom:{3}\">Công ty Cổ phần Cấp nước Thừa Thiên Huế;</div>" +
                            "<div style=\"font-style: italic; font-size:{1}; margin-left:120px; margin-bottom:{3}\">Theo đề nghị của bộ phận dịch vụ khách hàng của XNCN {15}</div>" +
                            "<br/>" +
                            "<table style=\"width:100%\">" +
                                "<tr>" +
                                    "<td style=\"text-align:center; font-size:{1}; font-weight:bold; ; margin-bottom:{3}\">" +
                                        "CHI NHÁNH (XÍ NGHIỆP CẤP NƯỚC {14}) THÔNG BÁO" +
                                    "</td>" +
                                "</tr>" +
                            "</table>" +
                            "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">1. Ngừng dịch vụ cấp nước đối với khách hàng có thông tin trên tại địa chỉ: </div>" +
                            "<div style=\"margin-left:130px; font-size:{1}; margin-bottom:{3}\">{5}</div>" +
                            "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">2. Thời gian ngừng dịch vụ cấp nước: kể từ ngày {8} tháng {9} năm {10}</div>" +
                            "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">3. Lý do: quá hạn thanh toán tiền nước kỳ: {6}</div>" +

                            "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">Việc cấp nước trở lại được thực hiện sau 24 giờ kể từ khi khách hàng thực hiện</div>" +
                            "<div style=\"margin-left:58px;; font-size:{1}; margin-bottom:{3}\">thanh toán tiền nước và chi phí liên quan (vật tư, nhân công đóng mở nước theo quy</div>" +
                            "<div style=\"margin-left:58px;; font-size:{1}; margin-bottom:{3}\">định hiện hành và các chi phí phát sinh khác).</div>" +

                            "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">Trường hợp khách hàng nhiều lần vi phạm nghĩa vụ thanh toán tiền nước, Công ty có</div>" +
                            "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">thể kéo dài thời gian ngừng dịch vụ cấp nước hoặc đơn phương chấm dứt hợp đồng, cắt hủy</div>" +
                            "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">đồng hồ đo đếm nước sạch; trường hợp khách hàng yêu cầu cung cấp nước sạch lại phải</div>" +
                            "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">chịu mọi chi phí phát sinh theo quy định bao gồm cả chi phí đồng hồ, …).</div>" +

                            "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">Xí nghiệp cấp nước {15} thông báo đến Quý khách hàng được biết, phối hợp thực hiện. </div>" +

                            "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">Rất mong nhận được sự hợp tác của Quý khách hàng.</div>" +
                            "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">Trung tâm CSKH (24/7, miễn phí cước) 1800 0036.</div>" +
                            "<table style=\"width:100%\">" +
                                "<tr>" +
                                    "<td style=\"text-align:right; font-size:{1}; font-weight:bold\">" +
                                           "GIÁM ĐỐC CHI NHÁNH" +
                                    "</td>" +
                                "</tr>" +

                            "</table>" +
                        "</div>"
                    ,
                    fontSize12,//0
                    fontSize13,//1
                    fontSize14,//2
                    lineSpace,//3
                    khConNo.Idkh,//4
                    khConNo.DiaChi,//5
                    kyHd,//6
                    khConNo.TenKH,//7
                    dateCupNuoc,//8
                    monthCupNuoc,//9
                    yearCupNuoc,//10
                    DateTime.Now.Day,//11
                    DateTime.Now.Month,//12
                    DateTime.Now.Year,//13
                    khConNo.XNCN.ToUpper(),//14
                    khConNo.XNCN);//15
                var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(htmlContent);
                return Convert.ToBase64String(pdfBytes);
            }
            public ResultLoginVnptCA LoginVnptCA(LoginEntity _request)
            {
                var result = new VnptHashDao().LoginVnptCA(_request);
                return result;
            }
            public string Sign(doc_sign docSign, KhConNo khConNo, string loginName)
            {
                var result = new VnptHashDao().Sign(docSign);
                if (result != null && result != "")
                {
                    UpdateGiayThongBaoDaKyDuyet(khConNo, loginName, result);
                }
                return "200";
            }
            public int LeaderPheDuyetThongBaoTamNgungCapNuoc(DieuKienLoc dieuKienLoc, string nguoiPheDuyet)
            {
                var listIdkh = dieuKienLoc.ListIDKH;
                if (!dieuKienLoc.isGetAll)
                {
                    if (listIdkh.Count() > 0)
                    {
                        for (var i = 0; i < listIdkh.Count(); i++)
                        {
                            try
                            {
                                //                        _db.ExecuteCommand(String.Format(@"INSERT INTO DongMoNuocOnline(NAM, THANG, IDKH, STATUS_DMNO, NV_Duyet_TBQHTN_1, NGAY_TBQHTN_2)
                                //                                                        SELECT '{0}', '{1}', '{2}', '{3}', '{4}', '{5}'
                                //                                                        WHERE NOT EXISTS(
                                //                                                            SELECT 1 FROM DongMoNuocOnline WHERE NAM = '{0}' AND THANG = '{1}' AND IDKH = '{2}'
                                //                                                        )", dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i], TBQH_1, nguoiPheDuyet, DateTime.Now.ToString("yyyy-MM-dd")));
                                //                        _db.ExecuteCommand(String.Format(@"MERGE INTO [DongMoNuocOnline] AS target
                                //                                                            USING (SELECT '{0}' AS NAM, '{1}' AS THANG, '{2}' AS IDKH) AS source
                                //                                                            ON (target.NAM = source.NAM AND target.THANG = source.THANG AND target.IDKH = source.IDKH)
                                //                                                            WHEN MATCHED THEN UPDATE SET target.STATUS_DMNO= '{3}', target.IsDeletedTBQH2 = NULL, target.LeaderDuyetTBQH2 = '{4}'
                                //                                                            WHEN NOT MATCHED THEN INSERT (NAM, THANG, IDKH, STATUS_DMNO, LeaderDuyetTBQH2, NGAY_TBQHTN_2) 
                                //                                                            VALUES (source.NAM, source.THANG, source.IDKH, '{3}', '{4}', '{5}');",
                                //                                                             dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i], TBQH_2, nguoiPheDuyet, DateTime.Now.ToString("yyyy-MM-dd")));
                                _db.ExecuteCommand(String.Format(@"UPDATE [DongMoNuocOnline] SET STATUS_DMNO = '{0}', IsDeletedTNCN = {1}, LeaderDuyetTNCN='{2}' 
                                                               WHERE NAM = {3} AND THANG = {4} AND IDKH = '{5}' AND ManagerDuyetTNCN IS NULL AND IsDeletedTNCN IS NULL AND PathThongBao_TNCN IS NOT NULL
                                                               AND DATEDIFF(day, NgayNhapCS, '{6}') = {7}",
                                                               TBTNCN, "NULL", nguoiPheDuyet, dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i], dieuKienLoc.NgayLoc, ThongTinQuyTrinh.NgayTNCN));

                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    try
                    {
                        _db.ExecuteCommand(String.Format(@"UPDATE [DongMoNuocOnline] SET STATUS_DMNO = '{0}', IsDeletedTNCN = {1}, LeaderDuyetTNCN='{2}' 
                                                               WHERE NAM = {3} AND THANG = {4} AND XNCN = N'{5}' AND ManagerDuyetTNCN IS NULL AND IsDeletedTNCN IS NULL AND PathThongBao_TNCN IS NOT NULL
                                                               AND DATEDIFF(day, NgayNhapCS, '{6}') = {7}",
                                                                  TBTNCN, "NULL", nguoiPheDuyet, dieuKienLoc.NamHd, dieuKienLoc.KyHd, dieuKienLoc.XNCN, dieuKienLoc.NgayLoc, ThongTinQuyTrinh.NgayTNCN));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                }

                return 200;
            }
            public int ManagerPheDuyetThongBaoTamNgungCapNuoc(DieuKienLoc dieuKienLoc, string nguoiPheDuyet)
            {
                var listIdkh = dieuKienLoc.ListIDKH;
                if (!dieuKienLoc.isGetAll)
                {
                    if (listIdkh.Count() > 0)
                    {
                        for (var i = 0; i < listIdkh.Count(); i++)
                        {
                            try
                            {
                                //                        _db.ExecuteCommand(String.Format(@"INSERT INTO DongMoNuocOnline(NAM, THANG, IDKH, STATUS_DMNO, NV_Duyet_TBQHTN_1, NGAY_TBQHTN_2)
                                //                                                        SELECT '{0}', '{1}', '{2}', '{3}', '{4}', '{5}'
                                //                                                        WHERE NOT EXISTS(
                                //                                                            SELECT 1 FROM DongMoNuocOnline WHERE NAM = '{0}' AND THANG = '{1}' AND IDKH = '{2}'
                                //                                                        )", dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i], TBQH_1, nguoiPheDuyet, DateTime.Now.ToString("yyyy-MM-dd")));
                                //                        _db.ExecuteCommand(String.Format(@"MERGE INTO [DongMoNuocOnline] AS target
                                //                                                            USING (SELECT '{0}' AS NAM, '{1}' AS THANG, '{2}' AS IDKH) AS source
                                //                                                            ON (target.NAM = source.NAM AND target.THANG = source.THANG AND target.IDKH = source.IDKH)
                                //                                                            WHEN MATCHED THEN UPDATE SET target.STATUS_DMNO= '{3}', target.IsDeletedTBQH2 = NULL, target.ManagerDuyetTBQH2 = '{4}'
                                //                                                            WHEN NOT MATCHED THEN INSERT (NAM, THANG, IDKH, STATUS_DMNO, ManagerDuyetTBQH2, NGAY_TBQHTN_2) 
                                //                                                            VALUES (source.NAM, source.THANG, source.IDKH, '{3}', '{4}', '{5}');",
                                //                                                             dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i], TBQH_2, nguoiPheDuyet, DateTime.Now.ToString("yyyy-MM-dd")));
                                _db.ExecuteCommand(String.Format(@"UPDATE [DongMoNuocOnline] SET STATUS_DMNO = '{0}', IsDeletedTNCN = {1}, ManagerDuyetTNCN='{2}' 
                                                               WHERE NAM = {3} AND THANG = {4} AND IDKH = '{5}' AND LeaderDuyetTNCN IS NOT NULL AND dmno.PathThongBao_TNCN IS NOT NULL
                                                                AND DATEDIFF(day, NgayNhapCS, '{6}') = {7}",
                                                                   TBTNCN, "NULL", nguoiPheDuyet, dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i], dieuKienLoc.NgayLoc, ThongTinQuyTrinh.NgayTNCN));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                        }
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    try
                    {
                        _db.ExecuteCommand(String.Format(@"UPDATE [DongMoNuocOnline] SET STATUS_DMNO = '{0}', IsDeletedTNCN = {1}, ManagerDuyetTNCN='{2}'
                                                        WHERE NAM = {3} AND THANG = {4} AND XNCN = N'{5}' AND LeaderDuyetTNCN IS NOT NULL AND PathThongBao_TNCN IS NOT NULL
                                                        AND DATEDIFF(day, NgayNhapCS, '{6}') = {7}",
                                                           TBTNCN, "NULL", nguoiPheDuyet, dieuKienLoc.NamHd, dieuKienLoc.KyHd, dieuKienLoc.XNCN, dieuKienLoc.NgayLoc, ThongTinQuyTrinh.NgayTNCN));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                return 200;
            }
            public int UpdateGiayThongBaoDaKyDuyet(KhConNo khConNo, string nguoiPheDuyet, string base64DaKy)
            {
                try
                {
                    var pathTBTNCN = SaveFile(khConNo, base64DaKy);
    //                _db.ExecuteCommand(String.Format(@"MERGE INTO [DongMoNuocOnline] AS target
    //                                                            USING (SELECT '{0}' AS NAM, '{1}' AS THANG, '{2}' AS IDKH) AS source
    //                                                            ON (target.NAM = source.NAM AND target.THANG = source.THANG AND target.IDKH = source.IDKH)
    //                                                            WHEN MATCHED THEN UPDATE SET target.STATUS_DMNO= '{3}', PathThongBao_TNCN = '{6}', ManagerDuyetTNCN = '{4}', NGAY_TNCN='{5}', IsKySoTNCN = 1
    //                                                            WHEN NOT MATCHED THEN INSERT (NAM, THANG, IDKH, STATUS_DMNO, ManagerDuyetTNCN, NGAY_TNCN, PathThongBao_TNCN, IsKySoTNCN) 
    //                                                            VALUES (source.NAM, source.THANG, source.IDKH, '{3}', '{4}', '{5}', '{6}', 1);",
    //                                                            khConNo.Nam, khConNo.Ky, khConNo.Idkh, TBTNCN, nguoiPheDuyet, DateTime.Now.ToString("yyyy-MM-dd"), pathTBTNCN));
                    _db.ExecuteCommand(String.Format(@"UPDATE [DongMoNuocOnline] SET STATUS_DMNO= '{3}', PathThongBao_TNCN = '{6}', ManagerDuyetTNCN = '{4}', NGAY_TNCN='{5}', IsKySoTNCN = 1
                                                                WHERE NAM = {0} AND THANG = {1} AND IDKH = '{2}' AND STATUS_DMNO = '{3}' AND PathThongBao_TNCN IS NOT NULL;",
                                                                khConNo.Nam, khConNo.Ky, khConNo.Idkh, TBTNCN, nguoiPheDuyet, DateTime.Now.ToString("yyyy-MM-dd"), pathTBTNCN));
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);    
                }
            
                return 200;
            }
            public List<KhConNo> GetKH_TNCN_DaPheDuyet(DieuKienLoc dieuKienLoc)
            {
                var isGuiApp = false;
                if (dieuKienLoc.IsGuiZalo != null || dieuKienLoc.IsGuiAppCSKH != null)
                {
                    isGuiApp = true;
                }
                var ListResult = new List<KhConNo>();
                try
                {
                    if (!isGuiApp)
                    {
                        ListResult = _db.ExecuteQuery<KhConNo>(@"
                                                     SELECT
                                                            tt.IDKH as Idkh,
			                                                tt.TENKH as TenKH,
			                                                tt.NAM as Nam,
			                                                tt.THANG as Ky,
			                                                tt.M3TINHTIEN as M3TinhTien,
			                                                tt.NGAYNHAP as NgayNhapCS,
                                                            tt.TONGTIEN as TongTien,
                                                            kh.DIDONG1 as SoDienThoai,
			                                                kh.SONHA+', '+dp.TENDP+', '+kv.TENKV as DiaChi,
                                                            kv.TENHIEU as XNCN
                                                        FROM TIEUTHU as tt 
                                                        LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH" +
                                                        (isGuiApp ? @" LEFT JOIN DongMoNuocOnline_Message as dmnom ON dmnom.IDKH = tt.IDKH AND dmnom.NAM = tt.NAM AND dmnom.THANG = tt.THANG" : "") +
                                                        @" LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
                                                        LEFT JOIN DongMoNuocOnline as dmno ON dmno.IDKH = tt.IDKH AND dmno.IDKH = tt.IDKH AND dmno.NAM = tt.NAM AND dmno.THANG = tt.THANG
                                                        LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP" +
                                                            @" WHERE tt.HETNO = 0 
	                                                    AND tt.TONGTIEN > 0
	                                                    AND tt.NAM = " + dieuKienLoc.NamHd
                                                            + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                            + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                            + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                            + (dieuKienLoc.MaDuongPho != null ? String.Format(" AND dp.TENDP LIKE N'%{0}%'", dieuKienLoc.MaDuongPho) : "")
                                                            + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                            + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                            + (dieuKienLoc.IsGuiAppCSKH == null ? "" : dieuKienLoc.IsGuiAppCSKH == "true" ? " AND dmnom.IsGuiThongBaoTamNgungCapNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTNCN + "'" + " AND dmnom.IsGuiAppCSKH = 1 AND (dmnom.IsXoaApp = 0 OR dmnom.IsXoaApp IS NULL)" : dieuKienLoc.IsGuiAppCSKH == "false" ? " AND dmnom.IsGuiThongBaoTamNgungCapNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTNCN + "' AND ((dmnom.IsGuiAppCSKH = 0 OR dmnom.IsGuiAppCSKH IS NULL) OR (dmnom.IsXoaApp = 1))" : "")
                                                            + (dieuKienLoc.IsGuiZalo == null ? "" : dieuKienLoc.IsGuiZalo == "true" ? " AND dmnom.IsGuiThongBaoTamNgungCapNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTNCN + "'" + " AND dmnom.IsGuiZalo = 1" : dieuKienLoc.IsGuiZalo == "false" ? " AND dmnom.IsGuiThongBaoTamNgungCapNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTNCN + "' AND (dmnom.IsGuiZalo = 0 OR dmnom.IsGuiZalo IS NULL)" : "")
                                                            + " AND DATEDIFF(day, tt.NGAYNHAP, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTNCN)
                                                            + " AND dmno.PathThongBao_TNCN IS NOT NULL"
                                                            + " AND dmno.ManagerDuyetTNCN IS NOT NULL"
                                                            + " ORDER BY tt.NGAYNHAP DESC"
                                                            )
                                                .Select(x => new KhConNo
                                                {
                                                    Idkh = x.Idkh,
                                                    TenKH = x.TenKH,
                                                    Nam = x.Nam,
                                                    Ky = x.Ky,
                                                    M3TinhTien = x.M3TinhTien,
                                                    NgayNhapCS = x.NgayNhapCS,
                                                    NgayNhapCSStr = x.NgayNhapCS.ToString("dd/MM/yyyy"),
                                                    NgayThongBaoNhacNoStr = x.NgayNhapCS.AddDays((double)ThongTinQuyTrinh.NgayTNCN).ToString("dd/MM/yyyy"),
                                                    DiaChi = x.DiaChi,
                                                    TongTien = x.TongTien,
                                                    SoDienThoai = x.SoDienThoai,
                                                    XNCN = x.XNCN
                                                })
                                                .ToList();
                    }
                    else
                    {
                        var listKhConNoLastResult = new List<KhConNo>();
                        ListResult = _db.ExecuteQuery<KhConNo>(@"SELECT a.* FROM (SELECT
		                                                        tt.IDKH as Idkh,
		                                                        tt.TENKH as TenKH,
		                                                        tt.NAM as Nam,
		                                                        tt.THANG as Ky,
		                                                        tt.M3TINHTIEN as M3TinhTien,
		                                                        tt.NGAYNHAP as NgayNhapCS,
		                                                        tt.TONGTIEN as TongTien,
		                                                        kh.DIDONG1 as SoDienThoai,
		                                                        kh.SONHA+', '+dp.TENDP+', '+kv.TENKV as DiaChi,
		                                                        kv.TENHIEU as XNCN,
                                                                tt.HETNO,
		                                                        dmno.STATUS_DMNO
	                                                        FROM TIEUTHU as tt
	                                                        LEFT JOIN KHACHHANG as kh ON tt.IDKH = kh.IDKH
	                                                        LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
	                                                        LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP
	                                                        LEFT JOIN DongMoNuocOnline as dmno ON dmno.IDKH = tt.IDKH AND dmno.NAM = tt.NAM AND dmno.THANG = tt.THANG
	                                                        WHERE tt.HETNO = 0 
	                                                        AND tt.TONGTIEN > 0
	                                                        AND dmno.ManagerDuyetTNCN IS NOT NULL
                                                            AND dmno.PathThongBao_TNCN IS NOT NULL
                                                            AND tt.NAM = " + dieuKienLoc.NamHd
                                                            + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                            + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                            + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                            + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
                                                            + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                            + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                            + " AND dmno.STATUS_DMNO = " + "'" + TBTNCN + "'"
                                                            + " AND DATEDIFF(day, tt.NGAYNHAP, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTNCN)
                                                        + @" ) as a LEFT JOIN DongMoNuocOnline_Message as dmnom
                                                        ON a.Idkh = dmnom.IDKH AND a.Ky = dmnom.THANG AND a.Nam = dmnom.NAM AND a.STATUS_DMNO = dmnom.LoaiThongBao
                                                        WHERE a.HETNO = 0"
                                                        + (dieuKienLoc.IsGuiAppCSKH == null ? "" : dieuKienLoc.IsGuiAppCSKH == "true" ? " AND dmnom.IsGuiThongBaoTamNgungCapNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTNCN + "'" + " AND dmnom.IsGuiAppCSKH = 1 AND (dmnom.IsXoaApp = 0 OR dmnom.IsXoaApp IS NULL)" : dieuKienLoc.IsGuiAppCSKH == "false" ? (@"
	                                                                                                                                                                                                                                                                                                                                                         AND (
                                                                                                                                                                                                                                                                                                                                                            (
		                                                                                                                                                                                                                                                                                                                                                        dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                                                                                        AND dmnom.LoaiThongBao = 'TBTNCN'
		                                                                                                                                                                                                                                                                                                                                                        AND (
				                                                                                                                                                                                                                                                                                                                                                        (dmnom.IsGuiAppCSKH = 0 OR dmnom.IsGuiAppCSKH IS NULL)
				                                                                                                                                                                                                                                                                                                                                                        OR dmnom.IsXoaApp = 1
			                                                                                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                                                                                        OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                                                                                        (
		                                                                                                                                                                                                                                                                                                                                                        SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                                                                                        WHERE IDKH = a.IDKH 
		                                                                                                                                                                                                                                                                                                                                                        AND THANG = a.Ky
		                                                                                                                                                                                                                                                                                                                                                        AND NAM = a.NAM
		                                                                                                                                                                                                                                                                                                                                                        AND LoaiThongBao = 'TBTNCN'
                                                                                                                                                                                                                                                                                                                                                                AND IsGuiAppCSKH = 1 
                                                                                                                                                                                                                                                                                                                                                                AND IsXoaApp = 0
	                                                                                                                                                                                                                                                                                                                                                        ) IS NULL)
                                                                                                                                                                                                                                                                                                                                                        )") : "")
                                                        + (dieuKienLoc.IsGuiZalo == null ? "" : dieuKienLoc.IsGuiZalo == "true" ? " AND dmnom.IsGuiThongBaoTamNgungCapNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTNCN + "'" + " AND dmnom.IsGuiZalo = 1" : dieuKienLoc.IsGuiZalo == "false" ? (@"
	                                                                                                                                                                                                                                                                                         AND (
                                                                                                                                                                                                                                                                                            (
		                                                                                                                                                                                                                                                                                        dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                        AND dmnom.LoaiThongBao = 'TBTNCN'
		                                                                                                                                                                                                                                                                                        AND (
				                                                                                                                                                                                                                                                                                        (dmnom.IsGuiZalo = 0 OR dmnom.IsGuiZalo IS NULL)
			                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                        )
	                                                                                                                                                                                                                                                                                        OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                        (
		                                                                                                                                                                                                                                                                                        SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                        WHERE IDKH = a.IDKH 
		                                                                                                                                                                                                                                                                                        AND THANG = a.Ky
		                                                                                                                                                                                                                                                                                        AND NAM = a.NAM
		                                                                                                                                                                                                                                                                                        AND LoaiThongBao = 'TBTNCN'
                                                                                                                                                                                                                                                                                                AND IsGuiZalo = 1
	                                                                                                                                                                                                                                                                                        ) IS NULL)
                                                                                                                                                                                                                                                                                        )") : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == null && dieuKienLoc.IsGuiZalo == "false" ? " OR dmnom.LoaiThongBao IS NULL" : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == "false" && dieuKienLoc.IsGuiZalo == null ? " OR dmnom.LoaiThongBao IS NULL" : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == "false" && dieuKienLoc.IsGuiZalo == "false" ? " OR dmnom.LoaiThongBao IS NULL" : "")
                                                        )
                                                        .Select(x => new KhConNo
                                                        {
                                                            Idkh = x.Idkh,
                                                            TenKH = x.TenKH,
                                                            Nam = x.Nam,
                                                            Ky = x.Ky,
                                                            M3TinhTien = x.M3TinhTien,
                                                            NgayNhapCS = x.NgayNhapCS,
                                                            NgayNhapCSStr = x.NgayNhapCS.ToString("dd/MM/yyyy"),
                                                            NgayThongBaoNhacNoStr = x.NgayNhapCS.AddDays((double)ThongTinQuyTrinh.NgayTNCN).ToString("dd/MM/yyyy"),
                                                            DiaChi = x.DiaChi,
                                                            TongTien = x.TongTien,
                                                            SoDienThoai = x.SoDienThoai,
                                                            XNCN = x.XNCN
                                                        })
                                                        .ToList();
                        foreach (var khConNo in ListResult)
                        {
                            if (listKhConNoLastResult.Where(x => x.Idkh == khConNo.Idkh).FirstOrDefault() == null)
                            {
                                listKhConNoLastResult.Add(khConNo);
                            }
                        }
                        return listKhConNoLastResult;
                    }
                    


                    return ListResult;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
            //public List<KhConNo> GetAllGiayThongBaoTamNgungCapNuoc(List<KhConNo> listKhConNo, DieuKienLoc dieuKienLoc)
            //{
            //    try
            //    {
            //        if (!dieuKienLoc.isGetAll)
            //        {
            //            var listStringIdkh = "";
            //            for (var i = 0; i < listKhConNo.Count(); i++)
            //            {
            //                if (i != listKhConNo.Count() - 1)
            //                {
            //                    listStringIdkh += "'" + listKhConNo[i].Idkh + "'" + ",";
            //                }
            //                else
            //                {
            //                    listStringIdkh += "'" + listKhConNo[i].Idkh + "'";
            //                }
            //            }
            //            var result = _db.ExecuteQuery<KhConNo>(String.Format(@"SELECT PathThongBao_TNCN as PathThongBao FROM DongMoNuocOnline WHERE THANG = {1} AND NAM = {2} AND IDKH IN ({0}) AND PathThongBao_TNCN IS NOT NULL",
            //                                                                    listStringIdkh, listKhConNo[0].Ky, listKhConNo[0].Nam))
            //                                                    .Select(x => new KhConNo
            //                                                    {
            //                                                        base64GiayTB = ConvertFileToBase64(rootFolder + x.PathThongBao)
            //                                                    }).ToList();
            //            return result;
            //        }
            //        else
            //        {
            //            var listKhConNoo = _db.ExecuteQuery<KhConNo>(@"SELECT
            //                                        dmno.PathThongBao_TNCN as PathThongBao
            //                                    FROM DongMoNuocOnline as dmno
            //                                    JOIN TIEUTHU as tt ON dmno.IDKH = tt.IDKH AND dmno.NAM = tt.NAM AND dmno.THANG = tt.THANG
            //                                    JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
            //                                    JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
            //                                    JOIN DUONGPHO as dp ON dp.MADP = kh.MADP
            //                                    AND dmno.NGAY_TNCN IS NOT NULL
            //                                    AND dmno.ManagerDuyetTNCN IS NOT NULL
            //                                    AND dmno.PathThongBao_TNCN IS NOT NULL
            //                                    AND tt.NAM = " + dieuKienLoc.NamHd
            //                                    + " AND tt.THANG = " + dieuKienLoc.KyHd
            //                                    + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
            //                                    + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
            //                                    + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
            //                                    + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
            //                                    + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
            //                                    + " AND DATEDIFF(day, tt.NGAYNHAPCS, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTNCN)
            //                                    + (dieuKienLoc.isZaloAndApp == "false" ? " AND ((kh.ZALOUSERID IS NULL OR kh.ZALOUSERID = '') AND kh.isZNS = 0)"
            //                                        : dieuKienLoc.isZaloAndApp == "true" ? " AND ((kh.ZALOUSERID IS NOT NULL AND kh.ZALOUSERID <> '') OR kh.isZNS = 1)" : "")
            //                                    + " ORDER BY tt.IDKH DESC "
            //                                    + "OFFSET " + dieuKienLoc.Page + " ROWS FETCH NEXT " + totalPage + " ROWS ONLY "
            //                                    )
            //                                    .Select(x => new KhConNo
            //                                    {
            //                                        base64GiayTB = ConvertFileToBase64(rootFolder + x.PathThongBao)
            //                                    })
            //                                    .ToList();
            //            return listKhConNoo;
            //        }
                
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e);
            //    }
            //    return new List<KhConNo>();

            //}
            public KhConNo Count_TB_TNCN_DaPheDuyet(DieuKienLoc dieuKienLoc)
            {
                var isGuiApp = false;
                if (dieuKienLoc.IsGuiZalo != null || dieuKienLoc.IsGuiAppCSKH != null)
                {
                    isGuiApp = true;
                }
                var count = 0;
                try
                {
                    if (!isGuiApp)
                    {
                        count = _db.ExecuteQuery<int>(@"SELECT
                                                    COUNT(dmno.IDKH)
                                                FROM DongMoNuocOnline as dmno
                                                JOIN TIEUTHU as tt ON dmno.IDKH = tt.IDKH AND dmno.NAM = tt.NAM AND dmno.THANG = tt.THANG
                                                JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
                                                JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
                                                JOIN DUONGPHO as dp ON dp.MADP = kh.MADP"
                                                + (isGuiApp ? @" JOIN DongMoNuocOnline_Message as dmnom ON dmnom.IDKH = tt.IDKH AND dmnom.NAM = tt.NAM AND dmnom.THANG = tt.THANG" : "") +
                                                @" AND dmno.NGAY_TNCN IS NOT NULL
                                                AND dmno.ManagerDuyetTNCN IS NOT NULL
                                                AND dmno.PathThongBao_TNCN IS NOT NULL
                                                AND tt.NAM = " + dieuKienLoc.NamHd
                                                + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
                                                + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                + (dieuKienLoc.IsGuiAppCSKH == null ? "" : dieuKienLoc.IsGuiAppCSKH == "true" ? " AND dmnom.IsGuiThongBaoTamNgungCapNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTNCN + "'" + " AND dmnom.IsGuiAppCSKH = 1 AND (dmnom.IsXoaApp = 0 OR dmnom.IsXoaApp IS NULL)" : dieuKienLoc.IsGuiAppCSKH == "false" ? " AND dmnom.IsGuiThongBaoTamNgungCapNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTNCN + "' AND ((dmnom.IsGuiAppCSKH = 0 OR dmnom.IsGuiAppCSKH IS NULL) OR (dmnom.IsXoaApp = 1))" : "")
                                                + (dieuKienLoc.IsGuiZalo == null ? "" : dieuKienLoc.IsGuiZalo == "true" ? " AND dmnom.IsGuiThongBaoTamNgungCapNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTNCN + "'" + " AND dmnom.IsGuiZalo = 1" : dieuKienLoc.IsGuiZalo == "false" ? " AND dmnom.IsGuiThongBaoTamNgungCapNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTNCN + "' AND (dmnom.IsGuiZalo = 0 OR dmnom.IsGuiZalo IS NULL)" : "")
                                                + " AND DATEDIFF(day, tt.NGAYNHAP, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTNCN)
                                                + (dieuKienLoc.isZaloAndApp == "false" ? " AND ((kh.ZALOUSERID IS NULL OR kh.ZALOUSERID = '') AND kh.isZNS = 0)"
                                                    : dieuKienLoc.isZaloAndApp == "true" ? " AND ((kh.ZALOUSERID IS NOT NULL AND kh.ZALOUSERID <> '') OR kh.isZNS = 1)" : "")
                                                )
                                                .FirstOrDefault();
                    }
                    else
                    {
                        count = _db.ExecuteQuery<int>(@"SELECT COUNT(a.Idkh) FROM (SELECT
		                                                        tt.IDKH as Idkh,
		                                                        tt.TENKH as TenKH,
		                                                        tt.NAM as Nam,
		                                                        tt.THANG as Ky,
		                                                        tt.M3TINHTIEN as M3TinhTien,
		                                                        tt.NGAYNHAPCS as NgayNhapCS,
		                                                        tt.TONGTIEN as TongTien,
		                                                        kh.DIDONG1 as SoDienThoai,
		                                                        kh.SONHA+', '+dp.TENDP+', '+kv.TENKV as DiaChi,
		                                                        kv.TENHIEU as XNCN,
                                                                tt.HETNO,
		                                                        dmno.STATUS_DMNO
	                                                        FROM TIEUTHU as tt
	                                                        LEFT JOIN KHACHHANG as kh ON tt.IDKH = kh.IDKH
	                                                        LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
	                                                        LEFT JOIN DUONGPHO as dp ON dp.MADP = kh.MADP
	                                                        LEFT JOIN DongMoNuocOnline as dmno ON dmno.IDKH = tt.IDKH AND dmno.NAM = tt.NAM AND dmno.THANG = tt.THANG
	                                                        WHERE tt.HETNO = 0 
	                                                        AND tt.TONGTIEN > 0
	                                                        AND dmno.ManagerDuyetTNCN IS NOT NULL
                                                            AND dmno.PathThongBao_TNCN IS NOT NULL
                                                            AND tt.NAM = " + dieuKienLoc.NamHd
                                                            + " AND tt.THANG = " + dieuKienLoc.KyHd
                                                            + (dieuKienLoc.XNCN != null ? " AND kv.TENHIEU = N'" + dieuKienLoc.XNCN + "'" : "")
                                                            + (dieuKienLoc.Idkh != null ? " AND tt.IDKH = " + "'" + dieuKienLoc.Idkh + "'" : "")
                                                            + (dieuKienLoc.MaDuongPho != null ? " AND dp.TENDP LIKE " + "N'%" + dieuKienLoc.MaDuongPho + "%'" : "")
                                                            + (dieuKienLoc.KhuVuc != null ? " AND kv.MAKV IN " + "(" + dieuKienLoc.KhuVuc + ")" : "")
                                                            + (dieuKienLoc.MaLoTrinh != null ? " AND tt.MADP= " + "'" + dieuKienLoc.MaLoTrinh + "'" : "")
                                                            + " AND dmno.STATUS_DMNO = " + "'" + TBTNCN + "'"
                                                            + " AND DATEDIFF(day, tt.NGAYNHAPCS, " + (dieuKienLoc.NgayLoc == null ? " GETDATE()" : "'" + dieuKienLoc.NgayLoc + "'") + ")=" + (ThongTinQuyTrinh.NgayTNCN)
                                                        + @" ) as a LEFT JOIN DongMoNuocOnline_Message as dmnom
                                                        ON a.Idkh = dmnom.IDKH AND a.Ky = dmnom.THANG AND a.Nam = dmnom.NAM AND a.STATUS_DMNO = dmnom.LoaiThongBao
                                                        WHERE a.HETNO = 0"
                                                        + (dieuKienLoc.IsGuiAppCSKH == null ? "" : dieuKienLoc.IsGuiAppCSKH == "true" ? " AND dmnom.IsGuiThongBaoTamNgungCapNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTNCN + "'" + " AND dmnom.IsGuiAppCSKH = 1 AND (dmnom.IsXoaApp = 0 OR dmnom.IsXoaApp IS NULL)" : dieuKienLoc.IsGuiAppCSKH == "false" ? (@"
	                                                                                                                                                                                                                                                                                                                                                             AND (
                                                                                                                                                                                                                                                                                                                                                                (
		                                                                                                                                                                                                                                                                                                                                                            dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                                                                                            AND dmnom.LoaiThongBao = 'TBTNCN'
		                                                                                                                                                                                                                                                                                                                                                            AND (
				                                                                                                                                                                                                                                                                                                                                                            (dmnom.IsGuiAppCSKH = 0 OR dmnom.IsGuiAppCSKH IS NULL)
				                                                                                                                                                                                                                                                                                                                                                            OR dmnom.IsXoaApp = 1
			                                                                                                                                                                                                                                                                                                                                                            )
	                                                                                                                                                                                                                                                                                                                                                            )
	                                                                                                                                                                                                                                                                                                                                                            OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                                                                                            (
		                                                                                                                                                                                                                                                                                                                                                            SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                                                                                            WHERE IDKH = a.IDKH 
		                                                                                                                                                                                                                                                                                                                                                            AND THANG = a.Ky
		                                                                                                                                                                                                                                                                                                                                                            AND NAM = a.NAM
		                                                                                                                                                                                                                                                                                                                                                            AND LoaiThongBao = 'TBTNCN'
                                                                                                                                                                                                                                                                                                                                                                    AND IsGuiAppCSKH = 1 
                                                                                                                                                                                                                                                                                                                                                                    AND IsXoaApp = 0
	                                                                                                                                                                                                                                                                                                                                                            ) IS NULL)
                                                                                                                                                                                                                                                                                                                                                            )") : "")
                                                        + (dieuKienLoc.IsGuiZalo == null ? "" : dieuKienLoc.IsGuiZalo == "true" ? " AND dmnom.IsGuiThongBaoTamNgungCapNuoc = 1 AND dmnom.LoaiThongBao = " + "'" + TBTNCN + "'" + " AND dmnom.IsGuiZalo = 1" : dieuKienLoc.IsGuiZalo == "false" ? (@"
	                                                                                                                                                                                                                                                                                                AND (
                                                                                                                                                                                                                                                                                                (
		                                                                                                                                                                                                                                                                                            dmnom.IsGuiThongBaoTienNuoc = 1 
		                                                                                                                                                                                                                                                                                            AND dmnom.LoaiThongBao = 'TBTNCN'
		                                                                                                                                                                                                                                                                                            AND (
				                                                                                                                                                                                                                                                                                            (dmnom.IsGuiZalo = 0 OR dmnom.IsGuiZalo IS NULL)
			                                                                                                                                                                                                                                                                                            )
	                                                                                                                                                                                                                                                                                            )
	                                                                                                                                                                                                                                                                                            OR (LoaiThongBao IS NULL OR 
	                                                                                                                                                                                                                                                                                            (
		                                                                                                                                                                                                                                                                                            SELECT TOP 1 IDKH FROM DongMoNuocOnline_Message 
		                                                                                                                                                                                                                                                                                            WHERE IDKH = a.IDKH 
		                                                                                                                                                                                                                                                                                            AND THANG = a.Ky
		                                                                                                                                                                                                                                                                                            AND NAM = a.NAM
		                                                                                                                                                                                                                                                                                            AND LoaiThongBao = 'TBTNCN'
                                                                                                                                                                                                                                                                                                    AND IsGuiZalo = 1
	                                                                                                                                                                                                                                                                                            ) IS NULL)
                                                                                                                                                                                                                                                                                            )") : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == null && dieuKienLoc.IsGuiZalo == "false" ? " OR dmnom.LoaiThongBao IS NULL" : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == "false" && dieuKienLoc.IsGuiZalo == null ? " OR dmnom.LoaiThongBao IS NULL" : "")
                                                        + (dieuKienLoc.IsGuiAppCSKH == "false" && dieuKienLoc.IsGuiZalo == "false" ? " OR dmnom.LoaiThongBao IS NULL" : "")
                                                        )
                                                        .FirstOrDefault();
                    }
                    
                    var totalPages = Math.Ceiling((double)(count / totalPage));
                    return new KhConNo
                    {
                        totalKh = count,
                        totalPage = totalPages
                    };
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                return null;

            }
            //public string GetAllGiayThongBaoTamNgungCapNuoc_PDF(List<KhConNo> listKhConNo, DieuKienLoc dieuKienLoc)
            //{
            //    var listStr = new List<string>();


            //    //var gdxn = GetChuKiGDXN(listKhConNo[0].XNCN);
            //    //for (int i = 0; i < listKhConNo.Count(); i++)
            //    //{
            //    //    listStr.Add(SetGiayThongBaoQuaHan2(listKhConNo[i], gdxn));
            //    //}

            //    var listBase64 = GetAllGiayThongBaoTamNgungCapNuoc(listKhConNo, dieuKienLoc);
            //    for (int i = 0; i < listBase64.Count(); i++)
            //    {
            //        listStr.Add(listBase64[i].base64GiayTB);
            //    }
            //    return GhepBase64ToPDF(listStr);
            //}
            public string GetAllGiayThongBaoTamNgungCapNuoc_PDF(List<KhConNo> listKhConNo, DieuKienLoc dieuKienLoc)
            {
                var client = new RestClient(domainAPI + "api/tb/tncn");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = new
                {
                    ListKHConNo = listKhConNo,
                    DieuKienLoc = dieuKienLoc
                };
                request.AddJsonBody(body);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<string>(response.Content);
                }
                else
                {
                    return null;
                }
            }
            public int Leader_HuyPheDuyetTNCN(DieuKienLoc dieuKienLoc)
            {
                var listIdkh = dieuKienLoc.ListIDKH;
                if (listIdkh.Count() > 0)
                {
                    for (var i = 0; i < listIdkh.Count(); i++)
                    {
                        try
                        {
                            _db.ExecuteCommand(String.Format(@"UPDATE [DongmoNuocOnline] SET IsDeletedTNCN = 1, LeaderDuyetTNCN = NULL WHERE NAM = {0} AND THANG = {1} AND IDKH = '{2}' AND ManagerDuyetTNCN IS NULL",
                                                               dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i]));
                        }
                        catch (Exception e)
                        {
                            throw new Exception();
                        }

                    }
                }
                else
                {
                    return 0;
                }
                return 200;
            }
            public int Manager_HuyPheDuyetTNCN(DieuKienLoc dieuKienLoc)
            {
                var listIdkh = dieuKienLoc.ListIDKH;
                //huy hang loat
                if (dieuKienLoc.isGetAll)
                {
                    try
                    {
                        _db.ExecuteCommand(String.Format(@"UPDATE [DongmoNuocOnline] SET ManagerDuyetTNCN = NULL, LeaderDuyetTNCN = NULL, IsKySoTNCN = NULL WHERE NAM = {0} AND THANG = {1} AND XNCN = N'{2}' AND STATUS_DMNO = 'TBTNCN'",
                                                           dieuKienLoc.NamHd, dieuKienLoc.KyHd, dieuKienLoc.XNCN));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                //huy chi dinh tung khach hang
                else
                {
                    if (listIdkh.Count() > 0)
                    {
                        for (var i = 0; i < listIdkh.Count(); i++)
                        {
                            try
                            {
                                _db.ExecuteCommand(String.Format(@"UPDATE [DongmoNuocOnline] SET ManagerDuyetTNCN = NULL, LeaderDuyetTNCN = NULL, IsKySoTNCN = NULL WHERE NAM = {0} AND THANG = {1} AND IDKH = {2} AND STATUS_DMNO = 'TBTNCN'",
                                                                   dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i]));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }

                        }
                    }
                    else
                    {
                        return 0;
                    }
                }

                return 200;
            }
            public int Leader_Bo_HuyPheDuyetTNCN(DieuKienLoc dieuKienLoc)
            {
                var listIdkh = dieuKienLoc.ListIDKH;
                if (listIdkh.Count() > 0)
                {
                    for (var i = 0; i < listIdkh.Count(); i++)
                    {
                        try
                        {
                            _db.ExecuteCommand(String.Format(@"UPDATE [DongmoNuocOnline] SET IsDeletedTNCN = NULL, LeaderDuyetTNCN = NULL WHERE NAM = {0} AND THANG = {1} AND IDKH = {2} AND IsDeletedTNCN = 1",
                                                               dieuKienLoc.NamHd, dieuKienLoc.KyHd, listIdkh[i]));
                        }
                        catch (Exception e)
                        {
                            throw new Exception();
                        }

                    }
                }
                else
                {
                    return 0;
                }
                return 200;
            }
            private string SaveFile(KhConNo khConNo, string base64Str)
            {
                var client = new RestClient(domainAPI + "api/tb/save-ky-so");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = new
                {
                    KhConNo = khConNo,
                    Base64Str = base64Str
                };
                request.AddJsonBody(body);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<string>(response.Content);
                }
                else
                {
                    throw new Exception();
                    return null;
                }
            }
        #endregion

        public List<string> GetListXNCN()
        {
            return _db.KHUVUCs.Select(x => x.TENHIEU).Distinct().ToList();
        }
        public List<LoTrinh> GetAllLoTrinh()
        {
            var ListLoTrinh = _db.ExecuteQuery<LoTrinh>("SELECT MADP, TENDP FROM DUONGPHO").ToList();
            return ListLoTrinh;
        }
        public NhanVienPheDuyetTBQH CheckNhanVienPheDuyetTBQH(string loginName)
        {

            var nhanVienPheDuyetTBQH = _db.ExecuteQuery<NhanVienPheDuyetTBQH>(@"SELECT 
	                                                        nv.MANV,
	                                                        nvcv.MACV,
	                                                        pb.MAPB
                                                          FROM [dbo].[NHANVIEN] as nv
                                                          LEFT JOIN PHONGBAN as pb ON nv.MAPB = pb.MAPB
                                                          LEFT JOIN NHANVIENCONGVIEC as nvcv ON nvcv.MANV = nv.MANV
                                                          WHERE nv.MANV = {0}",
                                                           loginName).FirstOrDefault();
            if (nhanVienPheDuyetTBQH == null) return null;
            var maPB = nhanVienPheDuyetTBQH.MAPB;
            if (maPB == "CNH" || maPB == "CNHD" || maPB == "CNHP")
            {
                if (maPB == "CNH")
                {
                    nhanVienPheDuyetTBQH.MAPB = "Huế";
                }
                else if (maPB == "CNHD")
                {
                    nhanVienPheDuyetTBQH.MAPB = "Hương Điền";
                }
                else if (maPB == "CNHP")
                {
                    nhanVienPheDuyetTBQH.MAPB = "Hương Phú";
                }
                return nhanVienPheDuyetTBQH;
            }
            return null;
        }


        //private string GhepBase64ToPDF(List<string> listBase64)
        //{
        //    using (var outputPdfStream = new MemoryStream())
        //    {
        //        var pdfDocument = new iTextSharp.text.Document();

        //        var pdfCopy = new itextSharpPDF.PdfCopy(pdfDocument, outputPdfStream);

        //        pdfDocument.Open();

        //        foreach (var base64Str in listBase64)
        //        {
        //            byte[] pdfBytes = Convert.FromBase64String(base64Str);

        //            var inputPdf = new itextSharpPDF.PdfReader(pdfBytes);
        //            int pageCount = inputPdf.NumberOfPages;
        //            for (int i = 1; i <= pageCount; i++)
        //            {
        //                pdfCopy.AddPage(pdfCopy.GetImportedPage(inputPdf, 1));
        //            }
        //        }

        //        pdfDocument.Close();

        //        byte[] mergedPdfBytes = outputPdfStream.ToArray();
        //        return Convert.ToBase64String(mergedPdfBytes);
        //    }
        //}
        //private string ConvertFileToBase64(string pathFolder)
        //{
        //    var byteFile = File.ReadAllBytes(pathFolder);
        //    return Convert.ToBase64String(byteFile);
        //}
        //private string SaveFile(KhConNo khConNo, string base64Str)
        //{
        //    var yearFolder = rootFolder + "\\TNCN\\" + khConNo.Nam;
        //    if (!Directory.Exists(yearFolder))
        //    {
        //        Directory.CreateDirectory(yearFolder);
        //    }
        //    var thangFolder = rootFolder + "\\TNCN\\" + khConNo.Nam + "\\" + khConNo.Ky+"\\";
        //    if (!Directory.Exists(thangFolder))
        //    {
        //        Directory.CreateDirectory(thangFolder);
        //    }
        //    var fileName = @"TNCN-" + khConNo.Idkh + "-" + khConNo.Ky + "-" + khConNo.Nam + ".pdf";
        //    var saveFile = thangFolder + fileName;
        //    var bytePdf = Convert.FromBase64String(base64Str);

        //    if (File.Exists(saveFile))
        //    {
        //        File.Delete(saveFile);
        //    }
        //    FileStream stream = new FileStream(saveFile, FileMode.CreateNew);
        //    BinaryWriter writer = new BinaryWriter(stream);
        //    writer.Write(bytePdf, 0, bytePdf.Length);
        //    writer.Close();
        //    return "\\TNCN\\" + khConNo.Nam + "\\" + khConNo.Ky + "\\" + fileName;
        //}

//        private string SaveFile(KhConNo khConNo, string base64Str)
//        {
//            var client = new RestClient(domainAPI + "api/tb/save-ky-so");
//            var request = new RestRequest(Method.POST);
//            request.AddHeader("Content-Type", "application/json");
//            var body = new
//            {
//                KhConNo = khConNo,
//                Base64Str = base64Str
//            };
//            request.AddJsonBody(body);
//            IRestResponse response = client.Execute(request);
//            if (response.StatusCode == HttpStatusCode.OK)
//            {
//                return JsonConvert.DeserializeObject<string>(response.Content);
//            }
//            else
//            {
//                throw new Exception();
//                return null;
//            }
//        }
//        public List<LoTrinh> GetAllLoTrinh()
//        {
//            var ListLoTrinh = _db.ExecuteQuery<LoTrinh>("SELECT MADP, TENDP FROM DUONGPHO").ToList();
//            return ListLoTrinh;
//        }
//        //public InfoTB GetKH_NhacNo(DieuKienLoc dieuKienLoc)
//        //{
//        //}
//        public GDXN CheckGDXN(string loginName)
//        {
            
//            var checkGDXN = _db.ExecuteQuery<GDXN>(@"SELECT 
//	                                                        nv.MANV,
//	                                                        nvcv.MACV,
//	                                                        pb.MAPB
//                                                          FROM [dbo].[NHANVIEN] as nv
//                                                          LEFT JOIN PHONGBAN as pb ON nv.MAPB = pb.MAPB
//                                                          LEFT JOIN NHANVIENCONGVIEC as nvcv ON nvcv.MANV = nv.MANV
//                                                          WHERE nvcv.MACV = 'GDXN' AND nv.MANV = {0}",
//                                                           loginName).FirstOrDefault();
//            if (checkGDXN == null) return null;
//            return checkGDXN;
//        }
//        public NhanVienPheDuyetTBQH CheckNhanVienPheDuyetTBQH(string loginName)
//        {

//            var nhanVienPheDuyetTBQH = _db.ExecuteQuery<NhanVienPheDuyetTBQH>(@"SELECT 
//	                                                        nv.MANV,
//	                                                        nvcv.MACV,
//	                                                        pb.MAPB
//                                                          FROM [dbo].[NHANVIEN] as nv
//                                                          LEFT JOIN PHONGBAN as pb ON nv.MAPB = pb.MAPB
//                                                          LEFT JOIN NHANVIENCONGVIEC as nvcv ON nvcv.MANV = nv.MANV
//                                                          WHERE nv.MANV = {0}",
//                                                           loginName).FirstOrDefault();
//            if (nhanVienPheDuyetTBQH == null) return null;
//            var maPB = nhanVienPheDuyetTBQH.MAPB;
//            if (maPB == "CNH" || maPB == "CNHD" || maPB == "CNHP")
//            {
//                if (maPB == "CNH")
//                {
//                    nhanVienPheDuyetTBQH.MAPB = "Huế";
//                }else if(maPB=="CNHD"){
//                    nhanVienPheDuyetTBQH.MAPB = "Hương Điền";
//                }
//                else if (maPB == "CNHP")
//                {
//                    nhanVienPheDuyetTBQH.MAPB = "Hương Phú";
//                }
//                return nhanVienPheDuyetTBQH;
//            }
//            return null;
//        }
//        public int PheDuyetQuaHanTienNuoc(int namHD, int thangHD, string MaPB)
//        {
//            //var gdxn = CheckGDXN(loginName);
//            //if (checkGDXN == false) return -2;//ko du quyen
            
//            switch (MaPB)
//            {
//                case "CNH":
//                    return CapNhatListKHQuaHanTienNuoc("Huế", namHD, thangHD);
//                case "CNHD":
//                    return CapNhatListKHQuaHanTienNuoc("Hương Điền", namHD, thangHD);
//                case "CNHP":
//                    return CapNhatListKHQuaHanTienNuoc("Hương Phú", namHD, thangHD);
//            }
//            return -1;//khong truc thuoc xn
//        }
            //public List<string> GetListXNCN()
            //{
            //    return _db.KHUVUCs.Select(x => x.TENHIEU).Distinct().ToList();
            //}
//        public string GetTenGDXN(string tenHieu)
//        {
//            var result = _db.KHUVUCs.Where(x => x.TENHIEU == tenHieu).Select(x => x.GIAMDOCCN).FirstOrDefault();
//            return result;
//        }
        
//        public byte[] TaoThongBaoQuaHanTienNuocNotZipNotCreate(string originTBQHTN, List<InfoTB> listINfoTB, string originPath)
//        {
//            Table template = null;
//            Body body = null;
//            using (WordprocessingDocument templateDoc = WordprocessingDocument.Open(originTBQHTN, true))
//            {
//                var mainPartTempl = templateDoc.MainDocumentPart;
//                body = mainPartTempl.Document.Elements<Body>().First();
//                template = mainPartTempl.Document.Body.Elements<Table>().First();
//            }
//            using (MemoryStream stream = new MemoryStream())
//            {
//                using (WordprocessingDocument targetDoc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
//                {
//                    var ngayHT = DateTime.Now.Day.ToString();
//                    var thangHT = DateTime.Now.Month.ToString();
//                    var namHT = DateTime.Now.Year.ToString();
//                    MainDocumentPart mainPart = targetDoc.AddMainDocumentPart();
//                    mainPart.Document = new Document();
//                    mainPart.Document.Append(body.CloneNode(true));
//                    Body doc = mainPart.Document.Body;
//                    string pathChuKi = "";
//                    //if (listINfoTB.Count > 0)
//                    //{
//                    //    if (listINfoTB[0].XNCN == "Huế")
//                    //    {
//                    //        pathChuKi = originPath + @"\BienBan\ChuKiGDXNCNHUE.png";
//                    //    }
//                    //    else if (listINfoTB[0].XNCN == "Hương Điền")
//                    //    {
//                    //        pathChuKi = originPath + @"\BienBan\ChuKiGDXNCNHUONGDIEN.png";
//                    //    }
//                    //    else if (listINfoTB[0].XNCN == "Hương Phú")
//                    //    {
//                    //        pathChuKi = originPath + @"\BienBan\ChuKiGDXNCNHUONGPHU.png";
//                    //    }
//                    //}
//                    if (listINfoTB.Count > 0)
//                    {
//                        pathChuKi = GetChuKiGDXN(listINfoTB[0].XNCN).ChuKi;
//                    }

//                    for (int i = 0; i < listINfoTB.Count; i++)
//                    {
//                        var ngayTT = listINfoTB[i].NgayNhap.AddDays(18);
//                        if (i == 0)
//                        {
//                            var preText = targetDoc.MainDocumentPart.Document.Descendants<Text>();
//                            ReplaceParamTBQHTN(preText, listINfoTB[i], ngayHT, thangHT, namHT, ngayTT);
//                        }
//                        else
//                        {
//                            //Phan trang
//                            Paragraph phanTrang = new Paragraph(new Run(new Break() { Type = BreakValues.Page }));
//                            doc.Append(phanTrang);
//                            doc.Append(template.CloneNode(true));
//                            var ListText = targetDoc.MainDocumentPart.Document.Descendants<Text>();
//                            ReplaceParamTBQHTN(ListText, listINfoTB[i], ngayHT, thangHT, namHT, ngayTT);
//                        }
//                        Table tableChuKy = mainPart.Document.Body.Descendants<Table>().ElementAt(3 + 4 * i);
//                        if (tableChuKy != null)
//                        {
//                            TableRow row = tableChuKy.Elements<TableRow>().ElementAt(0);
//                            row.Elements<TableCell>().ElementAt(0).AppendChild(InsertImage(pathChuKi, mainPart));
//                        }
//                        //if (i >= 10) break;
//                    }
//                    targetDoc.MainDocumentPart.Document.Save();

//                }
//                stream.Position = 0;
//                return stream.ToArray();
//            }
//        }
//        public List<TNCN> GetListDuyetTNCN(string tenXiNghiep, int namHD, int thangHD, string Idkh, string duongPho, string kvlist)
//        {
            
//            List<object> listParam = new List<object>() { tenXiNghiep, namHD, thangHD };
//            var dkc = @" AND DATEDIFF(day, tt.NGAYNHAP, GETDATE()) >= 22
//                        AND DATEDIFF(day, tt.NGAYNHAP, GETDATE()) <= 60 
//                        AND NgayTNCN IS NULL
//	                    AND (tt.STATUSDMNO = 'TBQHTN' OR tt.STATUSDMNO = 'SMS_TBQHTN')
//                        ORDER BY tt.NGAYNHAP DESC";
//            var dkParam = "";

//            if (Idkh != null)
//            {
//                dkParam += @" AND tt.IDKH = {3}";
//                listParam.Add(Idkh);
//            }
//            if(duongPho!=null)
//            {
//               duongPho = duongPho.Replace("%", "");
//               dkParam += @" AND dp.TENDUONGLD LIKE "+"N'%"+duongPho+"%'";
                
//            }
//            if (kvlist != null)
//            {
//                dkParam += @" AND kv.MAKV IN " + "(" + kvlist + ")";
//                //listParam.Add(kvlist);
//            }
            
//            var ListKHDuyetTBTN = _db.ExecuteQuery<TNCN>(@"
//                                                 SELECT
//                                                        tt.IDKH as Idkh,
//			                                            tt.TENKH as TenKh,
//			                                            tt.NAM as Nam,
//			                                            tt.THANG as Ky,
//			                                            tt.M3TINHTIEN as M3TinhTien,
//			                                            tt.NGAYNHAP as NgayNhap,
//			                                            kh.SONHA+', '+dp.TENDUONGLD+', '+kv.TENKV as DiaChi,
//                                                        tt.TONGTIEN as TongTien
//                                                    FROM TIEUTHU as tt 
//                                                    LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
//                                                    LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
//                                                    LEFT JOIN DUONGPHOLD as dp ON dp.MADPLD = kh.MADPLD
//	                                                WHERE kv.TENHIEU = {0}
//                                                    AND (STATUSDMNO = 'TBQHTN' OR STATUSDMNO = 'SMS_TBQHTN')
//                                                    AND Base64TNCN IS NULL
//	                                                AND HETNO = 0 
//	                                                AND TONGTIEN > 0
//	                                                AND NAM = {1}
//	                                                AND THANG = {2}"+dkParam+dkc,
//                                        listParam.ToArray())
//                                        .Select(x => new TNCN
//                                        { 
//                                           Idkh = x.Idkh,
//                                           TenKH = x.TenKH,
//                                           Nam = x.Nam,
//                                           Ky = x.Ky,
//                                           M3TinhTien = x.M3TinhTien,
//                                           NgayNhap = x.NgayNhap,
//                                           DiaChi = x.DiaChi,
//                                           TongTien = x.TongTien,
//                                           StrNgayNhap = x.NgayNhap.ToString("dd-MM-yyyy"),
//                                        })
//                                        .ToList();
            
//            return ListKHDuyetTBTN;
//        }
//        public int PheDuyetTamNgungCapNuoc(bool checkGDXN, int namHD, int thangHD, string MaPB)
//        {

//            //var gdxn = CheckGDXN(loginName);
//            //if (checkGDXN == false) return -2;//ko du quyen
//            switch (MaPB)
//            {
//                case "CNH":
//                    return CapNhatListKHTamNgungCapNuoc("Huế", namHD, thangHD);
//                case "CNHD":
//                    return CapNhatListKHTamNgungCapNuoc("Hương Điền", namHD, thangHD);
//                case "CNHP":
//                    return CapNhatListKHTamNgungCapNuoc("Hương Phú", namHD, thangHD);
//            }
//            return -1;
//        }
        
//        public string GetBase64PDFDuyetTNCN(string idkh, int ky, int nam, string xncn, string tenkh, string diaChi, string ngayNhap)
//        {
//            //var ChuKiGDXN = GetChuKiGDXN(xncn);
//            //if (ChuKiGDXN == null) return null;

//            var fontSize12 = "20px";
//            var fontSize13 = "21.66px";
//            var fontSize14 = "23.34px";
//            var lineSpace = "8px";
//            ngayNhap = String.Join("-", ngayNhap.Split('-').Reverse());
//            DateTime ngayCupNuoc = DateTime.Parse(ngayNhap).AddDays(22);

//            int dateCupNuoc = ngayCupNuoc.Day;
//            int monthCupNuoc = ngayCupNuoc.Month;
//            int yearCupNuoc = ngayCupNuoc.Year;

//            string kyHd = ky<10 ? "0"+ky+"/"+nam : ky + "/" +nam;
//            var htmlContent = String.Format(
//                    "<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\" />"+
//                        "<div style=\"color:white\">.</div>"+
//                        "<div style=\"margin-top:20px\">" +
//                        "<table style=\"width:100%; \">"+
//                            "<tr>"+
//                                "<td style=\"text-align:center\">"+
//                                   "<div style=\"font-size:{0};\">CÔNG TY CỔ PHẦN CẤP NƯỚC</div>"+  
//                                   "<div style=\"font-size:{0};\">THỪA THIÊN HUẾ</div>"+
//                                   "<div style=\"font-size:{0}; font-weight:bold; text-decoration: underline\">XÍ NGHIỆP CẤP NƯỚC {14}</div>" +
//                                   "<div style=\"font-size:{0};\">Số : ……/TB-XN…</div>"+
//                                "</td>"+
//                                "<td style=\"text-align:center; vertical-align: top\">" +
//                                   "<div style=\"font-size:{0}; font-weight:bold\">CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</div>" +
//                                   "<div style=\"font-size:{0}; font-weight:bold; text-decoration: underline\">Độc lập - Tự do - Hạnh phúc</div>" +
//                                   "<br/>"+
//                                   "<div style=\"font-size:{0}; font-style:italic\">Thừa Thiên Huế, ngày {11} tháng {12} năm {13} </div>" +
//                                "</td>"+
//                            "</tr>"+
//                        "</table>"+
//                        "<br/>" +
//                        "<table style=\"width:100%;\">" +
//                            "<tr>"+
//                                "<td style=\"text-align:center\">" +
//                                    "<div style=\"font-size:{2}; font-weight:bold\">THÔNG BÁO</div>" +
                                    
//                                "</td>"+
//                            "</tr>"+
//                            "<tr>" +
//                                "<td style=\"text-align:center\">" +
//                                    "<div style=\"font-size:{2}; font-weight:bold;\">V/v tạm ngừng dịch vụ cấp nước</div>" +
//                                    "<div style=\"color:white; border-bottom:1px solid black; width:100px; margin-left: 380px; margin-top: 10px\"></div>"+
//                                "</td>" +
//                            "</tr>" +
                    
//                            "<tr>" +
//                                "<td style=\"text-align:center\">" +
//                                    "<div style=\"font-size:{1}; margin-bottom:{3}\">Kính gửi: {7}</div>" +
//                                "</td>" +
//                            "</tr>" +
//                        "</table>"+
//                        "<div style=\"margin-left:120px\">" +
//                            "<div style=\"font-size:{1}; margin-bottom:{3}\">- Mã số khách hàng: {4}</div>" +
//                            "<div style=\"font-size:{1}; margin-bottom:{3}\">- Địa chỉ: {5}</div>" +
//                            "<div style=\"font-size:{1}; margin-bottom:{3}\">- Địa chỉ sử dụng nước: {5}</div>" +
//                            "<div style=\"font-style: italic; font-size:{1}; margin-bottom:{3}\">Căn cứ Điều 5 của Hợp đồng dịch vụ cấp nước sạch đã ký kết giữa Khách hàng và </div>" +
//                        "</div>"+
//                        "<div style=\"font-style: italic; margin-left:58px; font-size:{1}; margin-bottom:{3}\">Công ty Cổ phần Cấp nước Thừa Thiên Huế;</div>" +
//                        "<div style=\"font-style: italic; font-size:{1}; margin-left:120px; margin-bottom:{3}\">Theo đề nghị của …………………………………………………………...………</div>" +
//                        "<br/>"+
//                        "<table style=\"width:100%\">" +
//                            "<tr>"+
//                                "<td style=\"text-align:center; font-size:{1}; font-weight:bold; ; margin-bottom:{3}\">" +
//                                    "XÍ NGHIỆP CẤP NƯỚC {14} THÔNG BÁO"+
//                                "</td>"+
//                            "</tr>"+
//                        "</table>"+
//                        "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">1. Ngừng dịch vụ cấp nước đối với khách hàng có thông tin trên tại địa chỉ: </div>" +
//                        "<div style=\"margin-left:130px; font-size:{1}; margin-bottom:{3}\">{5}</div>" +
//                        "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">2. Thời gian ngừng dịch vụ cấp nước: kể từ ngày {8} tháng {9} năm {10}</div>" +
//                        "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">3. Lý do: quá hạn thanh toán tiền nước kỳ: {6}</div>" +

//                        "<div style=\"margin-left:120px; font-size:{1}; margin-bottom:{3}\">4. Việc cấp nước trở lại được thực hiện sau 24 giờ kể từ khi khách hàng thực hiện</div>" +
//                        "<div style=\"margin-left:58px;; font-size:{1}; margin-bottom:{3}\">thanh toán tiền nước và chi phí liên quan (vật tư, nhân công đóng mở nước theo quy</div>" +
//                        "<div style=\"margin-left:58px;; font-size:{1}; margin-bottom:{3}\">định hiện hành và các chi phí phát sinh khác).</div>" +

//                        "<div style=\"margin-left:125px; font-size:{1}; margin-bottom:{3}\">Trường hợp khách hàng nhiều lần vi phạm nghĩa vụ thanh toán tiền nước, Công ty có</div>" +
//                        "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">thể kéo dài thời gian ngừng dịch vụ cấp nước hoặc đơn phương chấm dứt hợp đồng, cắt hủy</div>" +
//                        "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">đồng hồ đo đếm nước sạch; trường hợp khách hàng yêu cầu cung cấp nước sạch lại phải</div>" +
//                        "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">chịu mọi chi phí phát sinh theo quy định bao gồm cả chi phí đồng hồ, …).</div>" +

//                        "<div style=\"margin-left:125px; font-size:{1}; margin-bottom:{3}\">Xí nghiệp cấp nước {15} thông báo đến Quý khách hàng được biết, phối hợp</div>" +
//                        "<div style=\"margin-left:58px; font-size:{1}; margin-bottom:{3}\">thực hiện. Rất mong nhận được sự hợp tác của Quý khách hàng.</div>" +
//                        "<table style=\"width:100%\">"+
//                            "<tr>"+
//                                "<td style=\"text-align:right; font-size:{1}; font-weight:bold\">" +
//                                       "GIÁM ĐỐC XÍ NGHIỆP"+
//                                "</td>"+
//                            "</tr>"+

//                        "</table>"+
//                    "</div>"
//                ,
//                fontSize12,
//                fontSize13,
//                fontSize14,
//                lineSpace,
//                idkh,
//                diaChi,
//                kyHd,
//                tenkh,
//                dateCupNuoc,
//                monthCupNuoc,
//                yearCupNuoc,
//                DateTime.Now.Day,
//                DateTime.Now.Month,
//                DateTime.Now.Year,
//                xncn.ToUpper(),
//                xncn);
//            var pdfBytes = (new NReco.PdfGenerator.HtmlToPdfConverter()).GeneratePdf(htmlContent);
//            return Convert.ToBase64String(pdfBytes);
//        }
//        public bool CapNhatDuyetCupNuoc(string idkh, int ky, int nam, string base64Str)
//        {
//           var khachHang = _db.ExecuteQuery<string>(@"SELECT idkh FROM TIEUTHU tt 
//                                                      WHERE tt.HETNO = 0
//                                                      AND tt.THANG = {1}
//                                                      AND tt.NAM = {2}
//                                                      AND tt.IDKH = {0}
//                                                      AND tt.TONGTIEN > 0", idkh, ky, nam).FirstOrDefault();
//           if (khachHang == null) return false;
//           _db.ExecuteCommand(@"UPDATE tt SET tt.NgayTNCN = CAST(GETDATE() AS DATE), tt.Base64TNCN = {0}, tt.STATUSDMNO = 'TBTNCN'
//                                FROM TIEUTHU as tt
//                                WHERE tt.IDKH = {1}
//                                AND tt.HETNO = 0
//                                AND tt.TONGTIEN > 0
//                                AND tt.THANG = {2}
//                                AND tt.NAM = {3}", base64Str, idkh, ky, nam);
//           return true;
//        }
//        public string GetListKhachHangTNCN(int ky, int nam, string xncn){
//            var listKhachHang = _db.ExecuteQuery<string>(@"SELECT tt.Base64TNCN 
//                                                            FROM TIEUTHU as tt 
//                                                            LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
//                                                            LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
//                                                            LEFT JOIN DUONGPHOLD as dp ON dp.MADPLD = kh.MADPLD
//                                                            WHERE kv.TENHIEU = {2}
//                                                            AND tt.HETNO = 0
//                                                            AND tt.TONGTIEN > 0
//                                                            AND tt.THANG = {0}
//                                                            AND tt.NAM = {1}
//                                                            AND tt.Base64TNCN IS NOT NULL", ky, nam, xncn).ToList();
//            if (listKhachHang.Count == 0) return null;
//            return GhepBase64ToPDF(listKhachHang);
        }
       
        
        //==================================================================================================================
//        private int CapNhatListKHQuaHanTienNuoc(string tenXiNghiep, int namHD, int thangHD)
//        {
//            var countKHQuaHanTienNuoc = _db.ExecuteQuery<int>(@"
//                                                SELECT COUNT(IDKH) FROM (
//                                                SELECT tt.IDKH
//                                                    FROM TIEUTHU as tt 
//                                                    LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
//                                                    LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
//                                                    LEFT JOIN DUONGPHOLD as dp ON dp.MADPLD = kh.MADPLD
//	                                                WHERE kv.TENHIEU = {0}
//	                                                AND HETNO = 0 
//	                                                AND TONGTIEN > 0
//	                                                AND NAM = {1}
//	                                                AND THANG = {2}
//	                                                AND DATEDIFF(day, tt.NGAYNHAP, GETDATE()) >= 18
//                                                    AND DATEDIFF(day, tt.NGAYNHAP, GETDATE()) <= 60 
//                                                    AND NgayTBQHTN IS NULL
//	                                                AND (
//		                                                (tt.STATUSDMNO <> 'TBQHTN' AND tt.STATUSDMNO <> 'SMS_TBQHTN'
//                                                        AND tt.STATUSDMNO <> 'TNCN' AND tt.STATUSDMNO <> 'SMS_TNCN') 
//		                                                OR tt.STATUSDMNO IS NULL
//	                                                )) as r",
//                                        tenXiNghiep,
//                                        namHD,
//                                        thangHD).FirstOrDefault();
//            if (countKHQuaHanTienNuoc > 0)
//            {
//                string nowDate = DateTime.Now.ToString("yyyy-MM-dd");
//                _db.ExecuteQuery<string>(@"UPDATE tt SET tt.STATUSDMNO = 'TBQHTN', NgayTBQHTN = {3}
//                                                FROM TIEUTHU as tt 
//                                                LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
//                                                LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
//                                                LEFT JOIN DUONGPHOLD as dp ON dp.MADPLD = kh.MADPLD
//	                                            WHERE kv.TENHIEU = {0}
//	                                            AND HETNO = 0 
//	                                            AND TONGTIEN > 0
//	                                            AND NAM = {1}
//	                                            AND THANG = {2}
//	                                            AND DATEDIFF(day, tt.NGAYNHAP, GETDATE()) >= 18
//                                                AND DATEDIFF(day, tt.NGAYNHAP, GETDATE()) <= 60
//                                                AND NgayTBQHTN IS NULL
//	                                            AND (
//		                                            (tt.STATUSDMNO <> 'TBQHTN'  AND tt.STATUSDMNO <> 'SMS_TBQHTN'
//                                                    AND tt.STATUSDMNO <> 'TNCN' AND tt.STATUSDMNO <> 'SMS_TNCN') 
//		                                            OR tt.STATUSDMNO IS NULL
//	                                            )",
//                                            tenXiNghiep,
//                                            namHD,
//                                            thangHD,
//                                            nowDate).ToList();
//                return countKHQuaHanTienNuoc;
//            }
//            return -1;
//        }
//        private int CapNhatListKHTamNgungCapNuoc(string tenXiNghiep, int namHD, int thangHD)
//        {
//            var countKHTamNgungCapNuoc = _db.ExecuteQuery<int>(@"
//                                                SELECT COUNT(IDKH) FROM (
//                                                SELECT tt.IDKH
//                                                    FROM TIEUTHU as tt 
//                                                      LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
//                                                      LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
//                                                      LEFT JOIN DUONGPHOLD as dp ON dp.MADPLD = kh.MADPLD
//	                                                    WHERE kv.TENHIEU = {0}
//	                                                    AND HETNO = 0 
//	                                                    AND TONGTIEN > 0
//	                                                    AND NAM = {1}
//	                                                    AND THANG = {2}
//	                                                    AND DATEDIFF(day, tt.NGAYNHAP, GETDATE()) >= 22 
//                                                        AND DATEDIFF(day, tt.NGAYNHAP, GETDATE()) <= 60 
//                                                        AND NgayTBQHTN IS NOT NULL
//	                                                    AND 
//                                                        (tt.STATUSDMNO = 'TBQHTN' OR tt.STATUSDMNO = 'SMS_TBQHTN')) as r",
//                                        tenXiNghiep,
//                                        namHD,
//                                        thangHD).FirstOrDefault();
//            string nowDate = DateTime.Now.ToString("yyyy-MM-dd");
//            if (countKHTamNgungCapNuoc> 0)
//            {
//                _db.ExecuteQuery<string>(@"UPDATE tt SET tt.STATUSDMNO = 'TNCN', tt.NgayTNCN = {3}
//                                          FROM TIEUTHU as tt 
//                                          LEFT JOIN  KHACHHANG as kh ON tt.IDKH = kh.IDKH
//                                          LEFT JOIN KHUVUC as kv ON kv.MAKV = kh.MAKV
//                                          LEFT JOIN DUONGPHOLD as dp ON dp.MADPLD = kh.MADPLD
//	                                        WHERE kv.TENHIEU = {0}
//	                                        AND HETNO = 0 
//	                                        AND TONGTIEN > 0
//	                                        AND NAM = {1}
//	                                        AND THANG = {2}
//	                                        AND DATEDIFF(day, tt.NGAYNHAP, GETDATE()) >= 22 
//                                            AND DATEDIFF(day, tt.NGAYNHAP, GETDATE()) <= 60 
//                                            AND NgayTBQHTN IS NOT NULL
//	                                        AND 
//                                            (tt.STATUSDMNO = 'TBQHTN' OR tt.STATUSDMNO = 'SMS_TBQHTN')",
//                                            tenXiNghiep,
//                                            namHD,
//                                            thangHD,
//                                            nowDate).ToList();
//                return countKHTamNgungCapNuoc;
//            }
//            return -1;
//        }
//        private void ReplaceParamTBQHTN(IEnumerable<Text> ListText, InfoTB infoTB, string ngayHT, string thangHT, string namHT, DateTime ngayTT)
//        {
//            List<string> listParam = new List<string> { "{NgayHT}", "{ThangHT}", "{NamHT}", "{TenKH}", "{DiaChi}", "{MaKH}", "{Ky}", "{Nam}", "{M3TinhTien}", "{TongTien}", "{NgayTT}", "{ThangTT}", "{NamTT}", "{TenXiNghiep}", "{GDXN}" };
//            List<string> listVariable = new List<string>{
//                            ngayHT, 
//                            thangHT,
//                            namHT, 
//                            infoTB.TenKH.ToUpper(), 
//                            infoTB.DiaChi.Replace("&", ""),
//                            infoTB.Idkh, 
//                            infoTB.Ky.ToString(), 
//                            infoTB.Nam.ToString(), 
//                            infoTB.M3TinhTien.ToString(), 
//                            infoTB.TongTien.ToString(), 
//                            ngayTT.Day.ToString(), 
//                            ngayTT.Month.ToString(),
//                            ngayTT.Year.ToString(),
//                            "XÍ NGHIỆP CẤP NƯỚC "+infoTB.XNCN.ToUpper(),
//                            infoTB.GDXNCN
//                    };
//            foreach (Text textElement in ListText)
//            {
//                for (var i = 0; i < listParam.Count; i++)
//                {
//                    textElement.Text = textElement.Text.Replace(listParam[i], listVariable[i]);
//                }
//            }
//        }
       
//        private Paragraph InsertImage(string base64Img, MainDocumentPart mainPart)
//        {

//            ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
//            var bytes = System.Convert.FromBase64String(base64Img);

//            //using (FileStream stream = new FileStream(pathImg, FileMode.Open))
//            using (Stream stream = new MemoryStream(bytes))
//            {
//                imagePart.FeedData(stream);
//            }

//            return AddImageToBody(mainPart, mainPart.GetIdOfPart(imagePart));
//        }
//        private Paragraph AddImageToBody(MainDocumentPart mainPart, string relationshipId)
//        {
//            // Define the reference of the image.
//            var element =
//                 new Drawing(
//                     new DW.Inline(
//                         //new DW.Extent() { Cx = 990000L, Cy = 792000L },
//                         new DW.Extent() { Cx = (4 / 2) * 914400, Cy = (4 / 2) * 914400 },
//                         new DW.EffectExtent()
//                         {
//                             LeftEdge = 0L,
//                             TopEdge = 0L,
//                             RightEdge = 0L,
//                             BottomEdge = 0L
//                         },
//                         new DW.DocProperties()
//                         {
//                             Id = (UInt32Value)1U,
//                             Name = "Picture 1"
//                         },
//                         new DW.NonVisualGraphicFrameDrawingProperties(
//                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
//                         new A.Graphic(
//                             new A.GraphicData(
//                                 new PIC.Picture(
//                                     new PIC.NonVisualPictureProperties(
//                                         new PIC.NonVisualDrawingProperties()
//                                         {
//                                             Id = (UInt32Value)0U,
//                                             Name = "New Bitmap Image.jpg"
//                                         },
//                                         new PIC.NonVisualPictureDrawingProperties()),
//                                     new PIC.BlipFill(
//                                         new A.Blip(
//                                             new A.BlipExtensionList(
//                                                 new A.BlipExtension()
//                                                 {
//                                                     Uri =
//                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
//                                                 })
//                                         )
//                                         {
//                                             Embed = relationshipId,
//                                             CompressionState =
//                                             A.BlipCompressionValues.Print
//                                         },
//                                         new A.Stretch(
//                                             new A.FillRectangle())),
//                                     new PIC.ShapeProperties(
//                                         new A.Transform2D(
//                                             new A.Offset() { X = 1L, Y = 1L },
//                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
//                                         new A.PresetGeometry(
//                                             new A.AdjustValueList()
//                                         )
//                                         { Preset = A.ShapeTypeValues.Rectangle }))
//                             )
//                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
//                     )
//                     {
//                         DistanceFromTop = (UInt32Value)0U,
//                         DistanceFromBottom = (UInt32Value)0U,
//                         DistanceFromLeft = (UInt32Value)0U,
//                         DistanceFromRight = (UInt32Value)0U,
//                         EditId = "50D07946"
//                     });

//            if (mainPart == null || mainPart.Document.Body == null)
//            {
//                throw new ArgumentNullException("MainDocumentPart and/or Body is null.");
//            }

//            return new Paragraph(new Run(element));
            
//        }
//        public GDXN GetChuKiGDXN(string XNCN)
//        {
//            var result =  _db.ExecuteQuery<GDXN>(@"SELECT 
//	                                            HOPDONGCHUKYNGUOIDAIDIEN as ChuKi,
//                                                GIAMDOCCN as TenGD
//                                            FROM [dbo].[KHUVUC] as kv
//                                            WHERE kv.TENHIEU = {0} AND HOPDONGCHUKYNGUOIDAIDIEN IS NOT NULL",
//                                            XNCN).FirstOrDefault();
//            if(XNCN=="Hương Điền"){
//                result.WidthChuKi = 395*2/3;
//                result.HeightChuKi = 276*2/3;
//            }
            
//            return result;
//        }
//        private TNCN GetKHTNCN(string idkh, int nam, int thang)
//        {
//            var ListTieuThu = _db.ExecuteQuery<TNCN>(@"SELECT 
//			                                            tt.IDKH as Idkh,
//			                                            tt.TENKH as TenKH,
//			                                            tt.NAM as Nam,
//			                                            tt.THANG as Ky,
//			                                            tt.TONGTIEN as TongTien,
//			                                            tt.M3TINHTIEN as M3TinhTien,
//			                                            tt.NGAYNHAP as NgayNhap,
//			                                            tt.STATUSDMNO as STATUSDMNO,
//                                                        kv.TENHIEU as XNCN,
//			                                            kh.SONHA+', '+dp.TENDUONGLD+', '+kv.TENKV as DiaChi,
//                                                        kv.HOPDONGCHUKYNGUOIDAIDIEN as CKGDXN      
//		                                            FROM TIEUTHU AS tt
//		                                            LEFT JOIN KHACHHANG AS kh 
//		                                            ON tt.IDKH = kh.IDKH
//		                                            LEFT JOIN DUONGPHOLD AS dp
//		                                            ON dp.MADPLD = kh.MADPLD
//		                                            LEFT JOIN KHUVUC AS kv
//		                                            ON kv.MAKV = kh.MAKV
//
//		                                            WHERE tt.HETNO = 0
//		                                            AND tt.TONGTIEN > 0
//		                                            AND tt.NAM = {0}
//		                                            AND tt.THANG = {1} 
//                                                    AND tt.IDKH = {2}",
//                                                   nam, thang, idkh).FirstOrDefault();
//            return ListTieuThu;
//        }
    //}
    public class ThongBaoRes
    {
        public string d { get; set; }
    }
    public class DieuKienLoc
    {
        public int KyHd { get; set; }
        public int NamHd { get; set; }
        public string Idkh { get; set; }
        public string KhuVuc { get; set; }
        public string XNCN { get; set; }
        public string MaDuongPho { get; set; }
        public string NgayLoc { get; set; }
        public string MaLoTrinh { get; set; }
        public List<string> ListIDKH { get; set; }
        public string isZaloAndApp{get;set;}
        public string IsGuiZalo { get; set; }
        public string IsGuiAppCSKH{get;set;}
        public bool isGetAll { get; set; }
        public int Page { get; set; }
    }
    public class ThongTinQuyTrinh
    {
        public static int NgayThongBaoTienNuoc = 3;
        public static int HanThanhToanTienNuoc = 5;

        public static int NgayThongBaoNhacNo = 6;
        public static int HanThanhToanNhacNo = 10;

        public static int NgayTBQHTT_1 = 11;
        public static int HanThanhToanQH_1 = 13;

        public static int NgayTBQHTT_2 = 14;
        public static int HanThanhToanQH_2 = 18;

        public static int NgayTNCN = 22;
    }
 
    public class GDXN
    {
         public string MANV{get;set;}
	     public string MACV{get;set;}
         public string MAPB { get; set; }
         public string ChuKi { get; set; }
         public string TenGD { get; set; }
         public int WidthChuKi { get; set; }
         public int HeightChuKi { get; set; }                        
    }
    public class NhanVienPheDuyetTBQH
    {
        public string MANV { get; set; }
        public string MACV { get; set; }
        public string MAPB { get; set; }   
    }
    public class InfoTB
    {
        public string Idkh { get; set; }
        public int Ky { get; set; }
        public int Nam { get; set; }
        public string TenKH { get; set; }
        public string DiaChi { get; set; }
        public long TongTien { get; set; }
        public long M3TinhTien { get; set; }
        public string XNCN { get; set; }
        public string GDXNCN { get; set; }
        public DateTime NgayNhap { get; set; }
        public string STATUSDMNO { get; set; }
        public string StrNgayNhap { get; set; }
    }
    public class LoTrinh
    {
        public string MaDP{get;set;}
        public string TenDP{get;set;}
    }
    public class KhConNo
    {
        public string Idkh { get; set; }
        public int Ky { get; set; }
        public int Nam { get; set; }
        public string TenKH { get; set; }
        public string DiaChi { get; set; }
        public double TongTien { get; set; }
        public int M3TinhTien { get; set; }
        public string NgayNhapCSStr { get; set; }
        public DateTime NgayNhapCS { get; set; }
        public string NgayThongBaoNhacNoStr { get; set; }
        public string SoDienThoai { get; set; }
        public string XNCN { get; set; }
        public string base64GiayTB { get; set; }
        public string PathThongBao { get; set; }
        public int totalKh { get; set; }
        public double totalPage { get; set; }
        public bool? IsDeletedTBQH2 { get; set; }
        public bool? IsDeletedTNCN { get; set; }
        public string ManagerDuyetTBQH2 { get; set; }
        public string ManagerDuyetTNCN { get; set; }
        public string LeaderPheDuyetTNCN { get; set; }
        public string LabelHuyPheDuyet { get; set; }
        public string LabelBoHuyPheDuyet { get; set; }
        public string LabelKySoTNCN { get; set; }
        public string LeaderPheDuyetTBQH2 { get; set; }
        public string PathThongBao_TNCN { get; set; }
        public bool? IsKySoTNCN { get; set; }
    }
    public class TNCN
    {
        public string Idkh { get; set; }
        public int Ky { get; set; }
        public int Nam { get; set; }
        public string TenKH { get; set; }
        public string DiaChi { get; set; }
        public double TongTien { get; set; }
        public int M3TinhTien { get; set; }
        public string XNCN { get; set; }
        public string GDXNCN { get; set; }
        public DateTime NgayNhap { get; set; }
        public string StrNgayNhap { get; set; }
        public string CKGDXN { get; set; }
    }
    public class TBQHTN
    {
        public string IDKH { get; set; }
        public int M3TinhTien { get; set; }
        public string TenKH { get; set; }
        public float TongTien { get; set; }
        public string DiaChi { get; set; }
        public int Ky { get; set; }
        public int Nam { get; set; }
        public string NgayNhap { get; set; }
        public string XNCN { get; set; }
        public string GDXN { get; set; }
    }
    
}
