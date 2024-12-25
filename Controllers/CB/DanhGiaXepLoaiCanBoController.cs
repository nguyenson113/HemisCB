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
    public class DanhGiaXepLoaiCanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        public DanhGiaXepLoaiCanBoController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //======================== TẠO LIST LẤY DỮ LIỆU TỪ API ============================
        private async Task<List<TbDanhGiaXepLoaiCanBo>> TbDanhGiaXepLoaiCanBos()
        {
            List<TbDanhGiaXepLoaiCanBo> tbDanhGiaXepLoaiCanBos = await ApiServices_.GetAll<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmDanhGiaCongChucVienChuc> dmdanhGiaCongChucVienChucs = await ApiServices_.GetAll<DmDanhGiaCongChucVienChuc>("/api/dm/DanhGiaCongChucVienChuc");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbDanhGiaXepLoaiCanBos.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdDanhGiaNavigation = dmdanhGiaCongChucVienChucs.FirstOrDefault(x => x.IdDanhGiaCongChucVienChuc == item.IdDanhGia);
                if (item.IdCanBoNavigation != null)
                    item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbDanhGiaXepLoaiCanBos;
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
            List<TbDanhGiaXepLoaiCanBo> getall = await TbDanhGiaXepLoaiCanBos();
            return View(getall);
        }

        // GET: DanhGiaXepLoaiCanBo


        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbDanhGiaXepLoaiCanBo> getall = await TbDanhGiaXepLoaiCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: DanhGiaXepLoaiCanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbDanhGiaXepLoaiCanBos = await TbDanhGiaXepLoaiCanBos();
                var tbDanhGiaXepLoaiCanBo = tbDanhGiaXepLoaiCanBos.FirstOrDefault(m => m.IdDanhGiaXepLoaiCanBo == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbDanhGiaXepLoaiCanBo == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbDanhGiaXepLoaiCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: DanhGiaXepLoaiCanBo/Create
       

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
                ViewData["IdDanhGia"] = new SelectList(await ApiServices_.GetAll<DmDanhGiaCongChucVienChuc>("/api/dm/DanhGiaCongChucVienChuc"), "IdDanhGiaCongChucVienChuc", "DanhGiaCongChucVienChuc");
                
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: DanhGiaXepLoaiCanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDanhGiaXepLoaiCanBo,IdCanBo,IdDanhGia,NamDanhGia,NganhDuocKhenThuong")] TbDanhGiaXepLoaiCanBo tbDanhGiaXepLoaiCanBo)
        {
            if (await TbDanhGiaXepLoaiCanBoExists(tbDanhGiaXepLoaiCanBo.IdDanhGiaXepLoaiCanBo)) ModelState.AddModelError("IdDanhGiaXepLoaiCanBo", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo", tbDanhGiaXepLoaiCanBo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDanhGia"] = new SelectList(await ApiServices_.GetAll<DmDanhGiaCongChucVienChuc>("/api/dm/DanhGiaCongChucVienChuc"), "IdDanhGiaCongChucVienChuc", "DanhGiaCongChucVienChuc", tbDanhGiaXepLoaiCanBo.IdDanhGia);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbDanhGiaXepLoaiCanBo.IdCanBo);
            return View(tbDanhGiaXepLoaiCanBo);
        }

        // GET: DanhGiaXepLoaiCanBo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDanhGiaXepLoaiCanBo = await ApiServices_.GetId<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo", id ?? 0);
            if (tbDanhGiaXepLoaiCanBo == null)
            {
                return NotFound();
            }
            ViewData["IdDanhGia"] = new SelectList(await ApiServices_.GetAll<DmDanhGiaCongChucVienChuc>("/api/dm/DanhGiaCongChucVienChuc"), "IdDanhGiaCongChucVienChuc", "DanhGiaCongChucVienChuc", tbDanhGiaXepLoaiCanBo.IdDanhGia);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDanhGiaXepLoaiCanBo.IdCanBo);
            return View(tbDanhGiaXepLoaiCanBo);
        }

        // POST: DanhGiaXepLoaiCanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDanhGiaXepLoaiCanBo,IdCanBo,IdDanhGia,NamDanhGia,NganhDuocKhenThuong")] TbDanhGiaXepLoaiCanBo tbDanhGiaXepLoaiCanBo)
        {
            if (id != tbDanhGiaXepLoaiCanBo.IdDanhGiaXepLoaiCanBo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo", id, tbDanhGiaXepLoaiCanBo);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbDanhGiaXepLoaiCanBoExists(tbDanhGiaXepLoaiCanBo.IdDanhGiaXepLoaiCanBo) == false)

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
            ViewData["IdDanhGia"] = new SelectList(await ApiServices_.GetAll<DmDanhGiaCongChucVienChuc>("/api/dm/DanhGiaCongChucVienChuc"), "IdDanhGiaCongChucVienChuc", "DanhGiaCongChucVienChuc", tbDanhGiaXepLoaiCanBo.IdDanhGia);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDanhGiaXepLoaiCanBo.IdCanBo);
            return View(tbDanhGiaXepLoaiCanBo);
        }

        // GET: DanhGiaXepLoaiCanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDanhGiaXepLoaiCanBos = await ApiServices_.GetAll<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo");
            var tbDanhGiaXepLoaiCanBo = tbDanhGiaXepLoaiCanBos.FirstOrDefault(m => m.IdDanhGiaXepLoaiCanBo == id);
            if (tbDanhGiaXepLoaiCanBo == null)
            {
                return NotFound();
            }

            return View(tbDanhGiaXepLoaiCanBo);
        }

        // POST: DanhGiaXepLoaiCanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbDanhGiaXepLoaiCanBoExists(int id)
        {
            var tbDanhGiaXepLoaiCanBos = await ApiServices_.GetAll<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo");
            return tbDanhGiaXepLoaiCanBos.Any(e => e.IdDanhGiaXepLoaiCanBo == id);
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

                List<TbDanhGiaXepLoaiCanBo> lst = new List<TbDanhGiaXepLoaiCanBo>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbDanhGiaXepLoaiCanBo> TbDanhGiaXepLoaiCanBos = await ApiServices_.GetAll<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbDanhGiaXepLoaiCanBo model = new TbDanhGiaXepLoaiCanBo();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdDanhGiaXepLoaiCanBo == id) || TbDanhGiaXepLoaiCanBos.Any(t => t.IdDanhGiaXepLoaiCanBo == id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdDanhGiaXepLoaiCanBo = id; // Gán ID
                    model.NamDanhGia = DateOnly.ParseExact(item[0], "yyyy", CultureInfo.InvariantCulture);
                    model.NganhDuocKhenThuong = item[1];
                    model.IdCanBo = ParseInt(item[2]);
                    model.IdDanhGia = ParseInt(item[3]);
                  
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbDanhGiaXepLoaiCanBo(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbDanhGiaXepLoaiCanBo(TbDanhGiaXepLoaiCanBo item)
        {
            await ApiServices_.Create<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo", item);
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
