/*
───────────────────────────────────────
  TRIỂN KHAI MÃ HÓA ĐỐI XỨNG BẰNG C# 
        TÁC GIẢ: TRƯỜNG CHINH 
───────────────────────────────────────

Chương trình nhỏ mô phỏng thuật toán mã hóa nhỏ tự build 

- Giới thiệu:
  + Mã hóa: là khái niệm luôn có 2 chiều là mã hóa <-> giải mã, có khóa để giải mã (key), độ dài không cố định  
    - Dùng để trao đổi dữ liệu mà không sợ bên thứ 3 đọc được 

  + Hàm băm (hash): là khái niệm 1 chiều, độ dài không đổi tùy thuật toán sẽ cho ra độ dài cố định khác nhau 
    - Dùng để kiểm tra tính toàn vẹn dữ liệu 


- Thành phần: 
  + P: nội dung (bản rõ)
  + C: nội dung sau mã hóa (bản mã)
  + K1: khóa 1
  + K2: khóa 2
  + R: số vòng lặp


- Các phép biến đổi
  + XOR: khác bit là 1 giống bit là 0 
  + NOT: đảo bit 1 là 0, 0 là 1
  + ROTL 3: xoay trái 3 bit | vd: 110 1110 -> 1110 110
  + ROTR 3: xoay phải 3 bit | vd: 110 1110 -> 110 1101
  + mod: phép chia lấy dư (%)


- Công thức lấy khóa:
  + K1[i] = K1[(i + k) mod |K1|]
  + K2[i] = K2[(i + k) mod |K2|] 
    -> trong đó K1[i] là byte thứ i trong vòng lặp
    -> k là vòng lặp thứ k của R 
    -> |K1|, |K2| là chiều dài khóa 1, 2
  ? Vì sao phải mod khúc này 
  Vì khóa thường ngắn hơn nội dung nên mod để lặp lại bằng với dữ liệu
  VD: noidung            = Nguyen Truong Chinh  
      khoa               = 1234
      sau khi mod khóa   = 1234123412341234123 (đủ chiều dài)  


- Công thức mã hóa:
  + Lặp R lần
    + C = P XOR K1[i]   Bước 1: C là nội dung ban đầu XOR với từng ký tự (byte) của Key 1
    + C = NOT(C)        Bước 2: C phủ định lại C
    + C = ROTL(C, 3)    Bước 3: C xoay trái 3 bit  
    + C = C XOR K2[i]   Bước 4: C XOR với từng ký tự (byte) của Key 2

  [*] Công thức chung: C = ROTL(NOT(P XOR K1[i]), 3) XOR K2[i]


- Công thức giải mã:
  + Lặp R lần
    + C = C XOR K2[i]   Bước 1: C XOR lại với từng ký tự (byte) cuủa Key 2 để quay lại trạng thái trước khi XOR 
    + C = ROTR(C, 3)    Bước 2: C xoay phải 3 bit 
    + C = NOT(C)        Bước 3: C phủ định lần nữa với C
    + P = C XOR K1[i]   Bước 4: C XOR với từng ký tự (byte) của Key 1 quay về nội dung ban đầu

  [*] Công thức chung:  P = NOT(ROTR(C XOR K2[i], 3)) XOR K1[i]


- Quá trình
  + Quá trình chạy sẽ chạy R vòng mã hóa và giải mã do người dùng nhập 
  + Quá trình mã hóa sẽ chạy từ vòng 0 -> R - 1
  + Riêng quá trình giải mã sẽ được thực hiện theo thứ tự vòng ngược lại từ (R - 1) -> 0


- Đút kết 
  + Quy trình tạo công thức mã hóa và giải mã cho thấy từng bước đối lập nhau để giải mã hóa được 

        MÃ HÓA     │     GIẢI MÃ   
     ──────────────┼───────────────
      - bước 1     │    - bước 4
      - bước 2     │    - bước 3
      - bước 3     │    - bước 2 
      - bước 4     │    - bước 1

*/

public class Program {
    public static void nhap_noidung(
        string loai, out string noidung, out string khoa1, out string khoa2, out int vonglap)
    {
        noidung = ""; khoa1 = ""; khoa2 = "";
        vonglap = 0;
        if(loai == "Mahoa" || loai == "Giaima"){
            Console.Write("\t(?) Nhập nội dung: "); 
            noidung = Console.ReadLine()!;
            Console.Write("\t(?) Nhập khóa 1: "); 
            khoa1 = Console.ReadLine()!;
            Console.Write("\t(?) Nhập khóa 2: "); 
            khoa2 = Console.ReadLine()!;
            Console.Write("\t(?) Nhập số lần lặp: "); 
            int.TryParse(Console.ReadLine()!, out vonglap);            
        } else if(loai == "Hash") {
            Console.Write("\t(?) Nhập nội dung: "); 
            noidung = Console.ReadLine()!;            
        } else {
            Console.WriteLine("(!) Không hỗ trợ loại này !!!");
        }
    }

    public static void dung_chuongtrinh(){
        Console.Write("\nNhập phím bất kỳ để tiếp tục ...");
        Console.ReadKey();
    }

    public static void Main(){

        string noidung, khoa1, khoa2;
        int vonglap;

        string menu = @"MÔ PHỎNG MÃ HÓA ĐỐI XỨNG

[01] Mã hóa nội dung
[02] Giải mã nội dung
[03] Hash nội dung
[04] Thoát
        ";
        while(true){
            Console.Clear();
            Console.WriteLine(menu);
            Console.Write("[?] Lựa chọn: ");
            int luachon;
            int.TryParse(Console.ReadLine()!, out luachon);
            if(luachon == 1){
                Console.WriteLine("\n[Mã hóa]");
                nhap_noidung("Mahoa", out noidung, out khoa1, out khoa2, out vonglap);
                byte[] vanbanmahoa = MaHoa.mahoa(noidung, khoa1, khoa2, vonglap);
                MaHoa.in_mahoa(vanbanmahoa);
            } 
            else if(luachon == 2){
                Console.WriteLine("\n[Giải mã]");
                nhap_noidung("Giaima", out noidung, out khoa1, out khoa2, out vonglap);
                string noidunggiaima = GiaiMa.giaima(noidung, khoa1, khoa2, vonglap);
                GiaiMa.in_giaima(noidunggiaima);
            }
            else if(luachon == 3){
                Console.WriteLine("\n[Hash]");
                nhap_noidung("Hash", out noidung, out khoa1, out khoa2, out vonglap);
                string hash_noidung = Sha256.hash(noidung);
                Sha256.in_hash(hash_noidung);
            }
            else if(luachon == 4){
                break;
            }
            else{
                Console.WriteLine("Vui lòng nhập đúng !!!");
            }
            dung_chuongtrinh();
        }
    }
}