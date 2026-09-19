using System.Text;
using System.Security.Cryptography;

public class Sha256 {
    // hàm hash sha256 (kiểm tra tính toàn vẹn dữ liệu)
    public static string hash(string noidung){
        byte[] bnoidung = Encoding.UTF8.GetBytes(noidung);
        byte[] hash_noidung = SHA256.HashData(bnoidung);
        return Convert.ToHexString(hash_noidung).ToLower();
    }

    public static void in_hash(string hash){
        Console.WriteLine("\t*[Hash] " + hash);
    }

    // public static void sosanh_hash(string hash1, string hash2){
    //     string sosanh_hash = (hash1 == hash2 ? "Toàn vẹn" : "Không toàn vẹn");
    //     Console.WriteLine("[Nội dung] " + sosanh_hash);
    // } 
}