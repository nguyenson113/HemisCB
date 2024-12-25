using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HemisCB.Models;
using HemisCB.API;
using HemisCB.Models.DM;
using Newtonsoft.Json;
using System.Globalization;

namespace HemisCB.Controllers.CB
{
    public class DienBienLuongController : Controller
    {
        private readonly ApiServices ApiServices_;

        public DienBienLuongController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //============================TẠO DANH SÁCH LẤY API========================
        private async Task<List<TbDienBienLuong>> TbDienBienLuongs()
        {
            // Lấy danh sách TbDienBienLuong từ API
            List<TbDienBienLuong> tbDienBienLuongs = await ApiServices_.GetAll<TbDienBienLuong>("/api/cb/DienBienLuong");
            // Lấy danh sách TbCanBo từ API
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            // Lấy danh sách DmHeSoLuong từ API
            List<DmHeSoLuong> dmheSoLuongs = await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong");
            // Lấy danh sách TbNguoi từ API
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            // Lấy danh sách DmTrinhDoDaoTao từ API
            List<DmTrinhDoDaoTao> dmtrinhDoDaoTaos = await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao");
            // Lặp qua từng phần tử trong danh sách TbDienBienLuong
            tbDienBienLuongs.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdHeSoLuongNavigation = dmheSoLuongs.FirstOrDefault(x => x.IdHeSoLuong == item.IdHeSoLuong);
                if (item.IdCanBoNavigation != null)
                    item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
                item.IdTrinhDoDaoTaoNavigation = dmtrinhDoDaoTaos.FirstOrDefault(x => x.IdTrinhDoDaoTao == item.IdTrinhDoDaoTao);
            });
            //Trả kết quả
            return tbDienBienLuongs;
        }

        private async Task<List<TbCanBo>> TbCanBos()
        {
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbcanbos.ForEach(item => {
                item.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdNguoi);

            });

            return tbcanbos;
        }

        public async Task<IActionResult> Statistics()
        {
            List<TbDienBienLuong> getall = await TbDienBienLuongs();
            return View(getall);
        }
        //=========================================Index================================================/
        // GET: DienBienLuong
        public async Task<IActionResult> Index()
        {   //Tạo try-catch bắt lỗi
            try
            {
                List<TbDienBienLuong> getall = await TbDienBienLuongs();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                //Trả về HTTP Error 400 
                return BadRequest();
            }

        }
        //======================================================Details=============================================//
        // GET: DienBienLuong/Details/5
        public async Task<IActionResult> Details(int? id)
        {  //Tạo khối try - catch trong Detail để bắt lỗi nếu có và trả về kết quả HTTp Error 400
            try
            {
                // Kiểm tra nếu id là null
                if (id == null)
            {
                return NotFound();
            }

            // Tìm quá trình công tác theo id
            var tbDienBienLuongs = await TbDienBienLuongs();
            var tbDienBienLuong = tbDienBienLuongs.FirstOrDefault(m => m.IdDienBienLuong == id);

            // Kiểm tra nếu không tìm thấy
            if (tbDienBienLuong == null)
            {
                return NotFound();
            }

            // Trả về view với thông tin chi tiết
            return View(tbDienBienLuong);
            }
            catch (Exception ex)
            {
                //Trả về HTTP Error 400
                return BadRequest();
            }
        }

        //=====================================Create=====================================//
        // GET: DienBienLuong/Create
      

        public async Task<IActionResult> Create()
        {
            //Tạo khối try - catch trong Create để bắt lỗi nếu có và trả về kết quả HTTp Error 400
            try
            {
                // Tạo danh sách lựa chọn cho IdCanBo, IdChucDanhGiangVien, IdChucVu
                ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao");
                ViewData["IdHeSoLuong"] = new SelectList(await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong"), "IdHeSoLuong", "HeSoLuong");
                //ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong1>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong");
                ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
                return View();
            }
            //Bắt lỗi nếu có và lưu lỗi vào BatLoi
            catch (Exception ex)
            {
                //Trả về lỗi HTTP Error 400
                return BadRequest();
            }
        }



        // POST: DienBienLuong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDienBienLuong,IdCanBo,IdTrinhDoDaoTao,NgayThangNam,IdBacLuong,IdHeSoLuong")] TbDienBienLuong tbDienBienLuong)
        {
            //Tạo khối try - catch trong Create để bắt lỗi nếu có và trả về kết quả HTTp Error 400
            try
            {
                if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbDienBienLuong>("/api/cb/DienBienLuong", tbDienBienLuong);
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbDienBienLuong.IdTrinhDoDaoTao);
            ViewData["IdHeSoLuong"] = new SelectList(await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong"), "IdHeSoLuong", "HeSoLuong", tbDienBienLuong.IdHeSoLuong);
            //ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong1>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong", tbDienBienLuong.IdBacLuong);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbDienBienLuong.IdCanBo);
            return View(tbDienBienLuong);
            }
            //Bắt lỗi nếu có và lưu lỗi vào BatLoi
            catch (Exception ex)
            {
                //Trả về lỗi HTTP Error 400
                return BadRequest();
            }

        }
        //=====================================================Edit=========================================//
        // GET: DienBienLuong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //Tạo khối try - catch  để bắt lỗi nếu có và trả về kết quả HTTp Error 400
            try
            {
                if (id == null)
            {
                return NotFound();
            }

            var tbDienBienLuong = await ApiServices_.GetId<TbDienBienLuong>("/api/cb/DienBienLuong", id ?? 0);
            if (tbDienBienLuong == null)
            {
                return NotFound();
            }
            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbDienBienLuong.IdTrinhDoDaoTao);
            ViewData["IdHeSoLuong"] = new SelectList(await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong"), "IdHeSoLuong", "HeSoLuong", tbDienBienLuong.IdHeSoLuong);
            //ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong1>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong", tbDienBienLuong.IdBacLuong);
            ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name", tbDienBienLuong.IdCanBo);
            return View(tbDienBienLuong);
            }
            //Bắt lỗi nếu có và lưu lỗi vào BatLoi
            catch (Exception ex)
            {
                //Trả về lỗi HTTP Error 400
                return BadRequest();
            }
        }

        // POST: DienBienLuong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDienBienLuong,IdCanBo,IdTrinhDoDaoTao,NgayThangNam,IdBacLuong,IdHeSoLuong")] TbDienBienLuong tbDienBienLuong)
        {
            //Tạo khối try - catch  để bắt lỗi nếu có và trả về kết quả HTTp Error 400
            try
            {
                if (id != tbDienBienLuong.IdDienBienLuong)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbDienBienLuong>("/api/cb/DienBienLuong", id, tbDienBienLuong);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbDienBienLuongExists(tbDienBienLuong.IdDienBienLuong) == false)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbDienBienLuong.IdTrinhDoDaoTao);
            ViewData["IdHeSoLuong"] = new SelectList(await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong"), "IdHeSoLuong", "HeSoLuong", tbDienBienLuong.IdHeSoLuong);
            //ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong1>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong", tbDienBienLuong.IdBacLuong);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDienBienLuong.IdCanBo);
            return View(tbDienBienLuong);
            }
            //Bắt lỗi nếu có và lưu lỗi vào BatLoi
            catch (Exception ex)
            {
                //Trả về lỗi HTTP Error 400
                return BadRequest();
            }
        }
        //===================================================================Delete=====================================//
        // GET: DienBienLuong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //Tạo khối try - catch  để bắt lỗi nếu có và trả về kết quả HTTp Error 400
            try
            {
            if (id == null)
            {
                return NotFound();
            }

            var tbDienBienLuongs = await TbDienBienLuongs();
            var tbDienBienLuong = tbDienBienLuongs.FirstOrDefault(m => m.IdDienBienLuong == id);
            if (tbDienBienLuong == null)
            {
                return NotFound();
            }

            return View(tbDienBienLuong);
            }
            //Bắt lỗi nếu có và lưu lỗi vào BatLoi
            catch (Exception ex)
            {
                //Trả về lỗi HTTP Error 400
                return BadRequest();
            }
        }

        // POST: DienBienLuong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {   //Tạo khối try-catch để bắt lỗi
            try
            {
                await ApiServices_.Delete<TbDienBienLuong>("/api/cb/DienBienLuong", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                //Trả về lỗi HTTP Error 400
                return BadRequest();
            }
        }

        private async Task<bool> TbDienBienLuongExists(int id)
        {
            var tbDienBienLuongs = await ApiServices_.GetAll<TbDienBienLuong>("/api/cb/DienBienLuong");
            return tbDienBienLuongs.Any(e => e.IdDienBienLuong == id);
        }


        //Import Excel 
        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                List<TbDienBienLuong> lst = new List<TbDienBienLuong>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbDienBienLuong> TbDienBienLuongs = await ApiServices_.GetAll<TbDienBienLuong>("/api/cb/DienBienLuong");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbDienBienLuong model = new TbDienBienLuong();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdDienBienLuong == id) || TbDienBienLuongs.Any(t => t.IdDienBienLuong == id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdDienBienLuong = id; // Gán ID
                    model.NgayThangNam = DateOnly.ParseExact(item[0], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.IdBacLuong = ParseInt(item[1]);
                    model.IdCanBo = ParseInt(item[2]);
                    model.IdHeSoLuong = ParseInt(item[3]);
                    model.IdTrinhDoDaoTao = ParseInt(item[4]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbDienBienLuong(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbDienBienLuong(TbDienBienLuong item)
        {
            await ApiServices_.Create<TbDienBienLuong>("/api/cb/DienBienLuong", item);
        }

        private int? ParseInt(string v)
        {
            if (int.TryParse(v, out int result)) // Nếu chuỗi có thể chuyển thành int
            {
                return result; // Trả về giá trị int
            }
            else
            {
                return null; // Nếu không thể chuyển thành int, trả về null
            }
        }
    }
}
