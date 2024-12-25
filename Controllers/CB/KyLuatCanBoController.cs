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
    public class KyLuatCanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        public KyLuatCanBoController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //==========================================TẠO DANH SÁCH THÔNG TIN TỪ API===========================
        private async Task<List<TbKyLuatCanBo>> TbKyLuatCanBos()
        {
            List<TbKyLuatCanBo> tbKyLuatCanBos = await ApiServices_.GetAll<TbKyLuatCanBo>("/api/cb/KyLuatCanBo");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmCapKhenThuong> dmcapKhenThuongs = await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong");
            List<DmLoaiKyLuat> dmloaiKyLuats = await ApiServices_.GetAll<DmLoaiKyLuat>("/api/dm/LoaiKyLuat");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbKyLuatCanBos.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdCapQuyetDinhNavigation = dmcapKhenThuongs.FirstOrDefault(x => x.IdCapKhenThuong == item.IdCapQuyetDinh);
                item.IdLoaiKyLuatNavigation = dmloaiKyLuats.FirstOrDefault(x => x.IdLoaiKyLuat == item.IdLoaiKyLuat);
                if (item.IdCanBoNavigation != null)
                    item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbKyLuatCanBos;
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
            List<TbKyLuatCanBo> getall = await TbKyLuatCanBos();
            return View(getall);
        }

        // GET: KyLuatCanBo
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbKyLuatCanBo> getall = await TbKyLuatCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: KyLuatCanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbKyLuatCanBos = await TbKyLuatCanBos();
                var tbKyLuatCanBo = tbKyLuatCanBos.FirstOrDefault(m => m.IdKyLuatCanBo == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbKyLuatCanBo == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbKyLuatCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }


        /// <summary>
        /// Hàm khởi tạo thông tin kỷ luật cán bộ
        /// phutn_8.10.2024
        /// </summary>
        /// <returns></returns>


        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
                ViewData["IdLoaiKyLuat"] = new SelectList(await ApiServices_.GetAll<DmLoaiKyLuat>("/api/dm/LoaiKyLuat"), "IdLoaiKyLuat", "LoaiKyLuat");
                ViewData["IdCapQuyetDinh"] = new SelectList(await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong"), "IdCapKhenThuong", "CapKhenThuong");
              
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: KyLuatCanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKyLuatCanBo,IdCanBo,IdLoaiKyLuat,LyDo,IdCapQuyetDinh,NgayThangNamQuyetDinh,SoQuyetDinh,NamBiKyLuat")] TbKyLuatCanBo tbKyLuatCanBo)
        {
            //Kiểm tra đã tồn tại trong TbKyLuatCanBo chua
            if (await TbKyLuatCanBoExists(tbKyLuatCanBo.IdKyLuatCanBo)) ModelState.AddModelError("IdKyLuatCanBo", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbKyLuatCanBo>("/api/cb/KyLuatCanBo", tbKyLuatCanBo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbKyLuatCanBo.IdCanBo);
            ViewData["IdCapQuyetDinh"] = new SelectList(await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong"), "IdCapKhenThuong", "CapKhenThuong", tbKyLuatCanBo.IdCapQuyetDinh);
            ViewData["IdLoaiKyLuat"] = new SelectList(await ApiServices_.GetAll<DmLoaiKyLuat>("/api/dm/LoaiKyLuat"), "IdLoaiKyLuat", "LoaiKyLuat", tbKyLuatCanBo.IdLoaiKyLuat);
            return View(tbKyLuatCanBo);
        }

        /// <summary>
        /// Khởi tạo sưa thông tin kỷ luật
        /// </summary>
        /// <param name="id"> là id định danh của Kỷ luật cán bộ trong cơ sở dữ liệu </param>
        /// <returns>View khởi tạo kỷ luật cán bộ</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbKyLuatCanBo = await ApiServices_.GetId<TbKyLuatCanBo>("/api/cb/KyLuatCanBo", id ?? 0);
            if (tbKyLuatCanBo == null)
            {
                return NotFound();
            }

            ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name", tbKyLuatCanBo.IdCanBo);
            ViewData["IdCapQuyetDinh"] = new SelectList(await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong"), "IdCapKhenThuong", "CapKhenThuong", tbKyLuatCanBo.IdCapQuyetDinh);
            ViewData["IdLoaiKyLuat"] = new SelectList(await ApiServices_.GetAll<DmLoaiKyLuat>("/api/dm/LoaiKyLuat"), "IdLoaiKyLuat", "LoaiKyLuat", tbKyLuatCanBo.IdLoaiKyLuat);
            return View(tbKyLuatCanBo);
        }

        // POST: KyLuatCanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdKyLuatCanBo,IdCanBo,IdLoaiKyLuat,LyDo,IdCapQuyetDinh,NgayThangNamQuyetDinh,SoQuyetDinh,NamBiKyLuat")] TbKyLuatCanBo tbKyLuatCanBo)
        {
            if (id != tbKyLuatCanBo.IdKyLuatCanBo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbKyLuatCanBo>("/api/cb/KyLuatCanBo", id, tbKyLuatCanBo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbKyLuatCanBoExists(tbKyLuatCanBo.IdKyLuatCanBo) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbKyLuatCanBo.IdCanBo);
            ViewData["IdCapQuyetDinh"] = new SelectList(await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong"), "IdCapKhenThuong", "CapKhenThuong", tbKyLuatCanBo.IdCapQuyetDinh);
            ViewData["IdLoaiKyLuat"] = new SelectList(await ApiServices_.GetAll<DmLoaiKyLuat>("/api/dm/LoaiKyLuat"), "IdLoaiKyLuat", "LoaiKyLuat", tbKyLuatCanBo.IdLoaiKyLuat);
            return View(tbKyLuatCanBo);
        }

        // GET: KyLuatCanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbKyLuatCanBos = await TbKyLuatCanBos();
            var tbKyLuatCanBo = tbKyLuatCanBos.FirstOrDefault(m => m.IdKyLuatCanBo == id);
            if (tbKyLuatCanBo == null)
            {
                return NotFound();
            }

            return View(tbKyLuatCanBo);
        }

        // POST: KyLuatCanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbKyLuatCanBo>("/api/cb/KyLuatCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbKyLuatCanBoExists(int id)
        {
            var tbKyLuatCanBos = await ApiServices_.GetAll<TbKyLuatCanBo>("/api/cb/KyLuatCanBo");
            return tbKyLuatCanBos.Any(e => e.IdKyLuatCanBo == id);
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

                List<TbKyLuatCanBo> lst = new List<TbKyLuatCanBo>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbKyLuatCanBo> TbKyLuatCanBos = await ApiServices_.GetAll<TbKyLuatCanBo>("/api/cb/KyLuatCanBo");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbKyLuatCanBo model = new TbKyLuatCanBo();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdKyLuatCanBo == id) || TbKyLuatCanBos.Any(t => t.IdKyLuatCanBo == id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdKyLuatCanBo = id; // Gán ID
                    model.NamBiKyLuat = DateOnly.ParseExact(item[2], "yyyy", CultureInfo.InvariantCulture);
                    model.SoQuyetDinh = item[1];
                    model.IdCanBo = ParseInt(item[0]);
                    model.IdCapQuyetDinh = ParseInt(item[3]);
                    model.IdLoaiKyLuat = ParseInt(item[4]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbKyLuatCanBo(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbKyLuatCanBo(TbKyLuatCanBo item)
        {
            await ApiServices_.Create<TbKyLuatCanBo>("/api/cb/KyLuatCanBo", item);
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
