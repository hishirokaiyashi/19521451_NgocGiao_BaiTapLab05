using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace OnThi_lab5.Models
{
    public class DataContext
    {
        public string ConnectionString { get; set; }
        public DataContext(string connectionstring)
        {
            this.ConnectionString = connectionstring;
        }
        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }
        /****************************************/
        public int sqlInsertCanHo(CanHoModel canho)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                var str = "insert into canho values(@macanho,@tenchuho)";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.Parameters.AddWithValue("macanho", canho.MaCanHo);
                cmd.Parameters.AddWithValue("tenchuho", canho.TenChuHo);
                return (cmd.ExecuteNonQuery());

            }
        }
        /****************************************/
        public List<object> sqlListByTimeNhanVien(int solan)
        {
            List<object> list = new List<object>();
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = @"select nv.manhanvien,nv.sodienthoai,count(*) as SoLan
                            from nhanvien nv join nv_bt on nv.manhanvien=nv_bt.manhanvien
                            group by nv.manhanvien, nv.sodienthoai
                            having count(*) >= @SoLanInput";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.Parameters.AddWithValue("SoLanInput", solan);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new
                        {
                            MaNV = reader["manhanvien"].ToString(),
                            SoDT= reader["sodienthoai"].ToString(),
                            SoLan=Convert.ToInt32(reader["SoLan"])
                        });
                    }
                    reader.Close(); 
                }
                conn.Close();
            }
            return list;
        }
        public List<NhanVienModel> sqlListNhanVien()
        {
            List<NhanVienModel> list = new List<NhanVienModel>();
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = @"select *from nhanvien";
                SqlCommand cmd = new SqlCommand(str, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new NhanVienModel
                        {
                            maNhanVien = reader["manhanvien"].ToString(),
                            tenNhanVien = reader["tennhanvien"].ToString(),
                            soDienThoai = reader["sodienthoai"].ToString(),
                            gioiTinh = Convert.ToBoolean(reader["gioitinh"])
                        });
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return list;
        }
        /****************************/
        public List<NVBTModel> sqlListThietBiByNhanVien(string manhanvien)
        {
            List<NVBTModel> list = new List<NVBTModel>();
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = @"select *from NV_BT where manhanvien=@MaNhanVien";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.Parameters.AddWithValue("MaNhanVien", manhanvien);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new NVBTModel()
                        {
                            maNhanVien = reader["manhanvien"].ToString(),
                            maCanHo = reader["macanho"].ToString(),
                            maThietBi = reader["mathietbi"].ToString(),
                            lanThu = reader["lanthu"].ToString(),
                            ngayBaoTri = Convert.ToDateTime(reader["ngaybaotri"])
                        }); 
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return list;
        }
        /*********************************/
        public int[] sqlDeleteThietBi(string mathietbi)
        {
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                var str1 = "delete from canho where macanho in (select distinct macanho from NV_BT where mathietbi=@MaThietBi)";
                SqlCommand cmd1 = new SqlCommand(str1, conn);
                cmd1.Parameters.AddWithValue("MaThietBi", mathietbi);
                //return (cmd1.ExecuteNonQuery());
                var str2 = "delete from thietbi where mathietbi=@MaThietBi";
                SqlCommand cmd2 = new SqlCommand(str2, conn);
                cmd2.Parameters.AddWithValue("MaThietBi", mathietbi);
                var str3 = "delete from nhanvien where manhanvien in (select distinct manhanvien from NV_BT where mathietbi=@MaThietBi)";
                SqlCommand cmd3 = new SqlCommand(str3, conn);
                cmd3.Parameters.AddWithValue("MaThietBi", mathietbi);
                var str4 = "delete from NV_BT where mathietbi=@mathietbi";
                SqlCommand cmd4 = new SqlCommand(str4, conn);
                cmd4.Parameters.AddWithValue("mathietbi", mathietbi);
                int[] a = new int[4];
                a[0] = cmd1.ExecuteNonQuery();
                a[1] = cmd2.ExecuteNonQuery();
                a[2] = cmd3.ExecuteNonQuery();
                a[3] = cmd4.ExecuteNonQuery();
                return a;
            }
        }
        public NVBTModel sqlViewThietBiByNhanVien(string mathietbi)
        {
            NVBTModel NVBT = new NVBTModel();
            using (SqlConnection conn = GetConnection())
            {
                conn.Open();
                string str = @"select * from NV_BT where mathietbi=@MaThietBi";
                SqlCommand cmd = new SqlCommand(str, conn);
                cmd.Parameters.AddWithValue("MaThietBi", mathietbi);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        NVBT.maNhanVien = reader["manhanvien"].ToString();
                        NVBT.maCanHo = reader["macanho"].ToString();
                        NVBT.maThietBi = reader["mathietbi"].ToString();
                        NVBT.lanThu = reader["lanthu"].ToString();
                        NVBT.ngayBaoTri = Convert.ToDateTime(reader["ngaybaotri"]);
                    }
                    reader.Close();
                }
                conn.Close();
            }
            return NVBT;
        }
    }
}
