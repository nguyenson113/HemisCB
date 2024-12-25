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
    public class QuaTrinhDaoTaoCuaCanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        public QuaTrinhDaoTaoCuaCanBoController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //==========================================TẠO DANH SÁCH THÔNG TIN TỪ API===========================
        private async Task<List<TbQuaTrinhDaoTaoCuaCanBo>> TbQuaTrinhDaoTaoCuaCanBos()
        {
            List<TbQuaTrinhDaoTaoCuaCanBo> tbQuaTrinhDaoTaoCuaCanBos = await ApiServices_.GetAll<TbQuaTrinhDaoTaoCuaCanBo>("/api/cb/QuaTrinhDaoTaoCuaCanBo");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmNganhDaoTao> dmnganhDaoTaos = await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao");
            List<DmLoaiHinhDaoTao> dmloaiHinhDaoTaos = await ApiServices_.GetAll<DmLoaiHinhDaoTao>("/api/dm/LoaiHinhDaoTao");
            List<DmTrinhDoDaoTao> dmtrinhDoDaoTaos = await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao");
            List<DmQuocTich> dmquocTichs = await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbQuaTrinhDaoTaoCuaCanBos.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdNganhDaoTaoNavigation = dmnganhDaoTaos.FirstOrDefault(x => x.IdNganhDaoTao == item.IdNganhDaoTao);
                item.IdLoaiHinhDaoTaoNavigation = dmloaiHinhDaoTaos.FirstOrDefault(x => x.IdLoaiHinhDaoTao == item.IdLoaiHinhDaoTao);
                item.IdTrinhDoDaoTaoNavigation = dmtrinhDoDaoTaos.FirstOrDefault(x => x.IdTrinhDoDaoTao == item.IdTrinhDoDaoTao);
                item.IdQuocGiaDaoTaoNavigation = dmquocTichs.FirstOrDefault(x => x.IdQuocTich == item.IdQuocGiaDaoTao);
                if (item.IdCanBoNavigation != null)
                    item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbQuaTrinhDaoTaoCuaCanBos;
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
            List<TbQuaTrinhDaoTaoCuaCanBo> getall = await TbQuaTrinhDaoTaoCuaCanBos();
            return View(getall);
        }


        // GET: QuaTrinhDaoTaoCuaCanBo
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbQuaTrinhDaoTaoCuaCanBo> getall = await TbQuaTrinhDaoTaoCuaCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: QuaTrinhDaoTaoCuaCanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbQuaTrinhDaoTaoCuaCanBos = await TbQuaTrinhDaoTaoCuaCanBos();
                var tbQuaTrinhDaoTaoCuaCanBo = tbQuaTrinhDaoTaoCuaCanBos.FirstOrDefault(m => m.IdQuaTrinhDaoTaoCuaCanBo == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbQuaTrinhDaoTaoCuaCanBo == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbQuaTrinhDaoTaoCuaCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // GET: QuaTrinhDaoTaoCuaCanBo/Create

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
                ViewData["IdLoaiHinhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmLoaiHinhDaoTao>("/api/dm/LoaiHinhDaoTao"), "IdLoaiHinhDaoTao", "LoaiHinhDaoTao");
                ViewData["IdNganhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao");
                ViewData["IdQuocGiaDaoTao"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc");
                ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
       

        // POST: QuaTrinhDaoTaoCuaCanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdQuaTrinhDaoTaoCuaCanBo,IdCanBo,IdTrinhDoDaoTao,IdQuocGiaDaoTao,CoSoDaoTao,ThoiGianBatDau,ThoiGianKetThuc,IdNganhDaoTao,NamTotNghiep,IdLoaiHinhDaoTao")] TbQuaTrinhDaoTaoCuaCanBo tbQuaTrinhDaoTaoCuaCanBo)
        {
            if (await TbQuaTrinhDaoTaoCuaCanBoExists(tbQuaTrinhDaoTaoCuaCanBo.IdQuaTrinhDaoTaoCuaCanBo)) ModelState.AddModelError("IdQuaTrinhDaoTaoCuaCanBo", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbQuaTrinhDaoTaoCuaCanBo>("/api/cb/QuaTrinhDaoTaoCuaCanBo", tbQuaTrinhDaoTaoCuaCanBo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbQuaTrinhDaoTaoCuaCanBo.IdCanBo);
            ViewData["IdLoaiHinhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmLoaiHinhDaoTao>("/api/dm/LoaiHinhDaoTao"), "IdLoaiHinhDaoTao", "LoaiHinhDaoTao", tbQuaTrinhDaoTaoCuaCanBo.IdLoaiHinhDaoTao);
            ViewData["IdNganhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbQuaTrinhDaoTaoCuaCanBo.IdNganhDaoTao);
            ViewData["IdQuocGiaDaoTao"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbQuaTrinhDaoTaoCuaCanBo.IdQuocGiaDaoTao);
            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbQuaTrinhDaoTaoCuaCanBo.IdTrinhDoDaoTao);
            return View(tbQuaTrinhDaoTaoCuaCanBo);
        }

        // GET: QuaTrinhDaoTaoCuaCanBo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbQuaTrinhDaoTaoCuaCanBo = await ApiServices_.GetId<TbQuaTrinhDaoTaoCuaCanBo>("/api/cb/QuaTrinhDaoTaoCuaCanBo", id ?? 0);
            if (tbQuaTrinhDaoTaoCuaCanBo == null)
            {
                return NotFound();
            }

            ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name", tbQuaTrinhDaoTaoCuaCanBo.IdCanBo);
            ViewData["IdLoaiHinhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmLoaiHinhDaoTao>("/api/dm/LoaiHinhDaoTao"), "IdLoaiHinhDaoTao", "LoaiHinhDaoTao", tbQuaTrinhDaoTaoCuaCanBo.IdLoaiHinhDaoTao);
            ViewData["IdNganhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbQuaTrinhDaoTaoCuaCanBo.IdNganhDaoTao);
            ViewData["IdQuocGiaDaoTao"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbQuaTrinhDaoTaoCuaCanBo.IdQuocGiaDaoTao);
            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbQuaTrinhDaoTaoCuaCanBo.IdTrinhDoDaoTao);
            return View(tbQuaTrinhDaoTaoCuaCanBo);
        }

        // POST: QuaTrinhDaoTaoCuaCanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdQuaTrinhDaoTaoCuaCanBo,IdCanBo,IdTrinhDoDaoTao,IdQuocGiaDaoTao,CoSoDaoTao,ThoiGianBatDau,ThoiGianKetThuc,IdNganhDaoTao,NamTotNghiep,IdLoaiHinhDaoTao")] TbQuaTrinhDaoTaoCuaCanBo tbQuaTrinhDaoTaoCuaCanBo)
        {
            if (id != tbQuaTrinhDaoTaoCuaCanBo.IdQuaTrinhDaoTaoCuaCanBo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbQuaTrinhDaoTaoCuaCanBo>("/api/cb/QuaTrinhDaoTaoCuaCanBo", id, tbQuaTrinhDaoTaoCuaCanBo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbQuaTrinhDaoTaoCuaCanBoExists(tbQuaTrinhDaoTaoCuaCanBo.IdQuaTrinhDaoTaoCuaCanBo) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbQuaTrinhDaoTaoCuaCanBo.IdCanBo);
            ViewData["IdLoaiHinhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmLoaiHinhDaoTao>("/api/dm/LoaiHinhDaoTao"), "IdLoaiHinhDaoTao", "LoaiHinhDaoTao", tbQuaTrinhDaoTaoCuaCanBo.IdLoaiHinhDaoTao);
            ViewData["IdNganhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbQuaTrinhDaoTaoCuaCanBo.IdNganhDaoTao);
            ViewData["IdQuocGiaDaoTao"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbQuaTrinhDaoTaoCuaCanBo.IdQuocGiaDaoTao);
            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbQuaTrinhDaoTaoCuaCanBo.IdTrinhDoDaoTao);
            return View(tbQuaTrinhDaoTaoCuaCanBo);
        }

        // GET: QuaTrinhDaoTaoCuaCanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbQuaTrinhDaoTaoCuaCanBos = await TbQuaTrinhDaoTaoCuaCanBos();
            var tbQuaTrinhDaoTaoCuaCanBo = tbQuaTrinhDaoTaoCuaCanBos.FirstOrDefault(m => m.IdQuaTrinhDaoTaoCuaCanBo == id);
            if (tbQuaTrinhDaoTaoCuaCanBo == null)
            {
                return NotFound();
            }

            return View(tbQuaTrinhDaoTaoCuaCanBo);
        }

        // POST: QuaTrinhDaoTaoCuaCanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbQuaTrinhDaoTaoCuaCanBo>("/api/cb/QuaTrinhDaoTaoCuaCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbQuaTrinhDaoTaoCuaCanBoExists(int id)
        {
            var tbQuaTrinhDaoTaoCuaCanBos = await ApiServices_.GetAll<TbQuaTrinhDaoTaoCuaCanBo>("/api/cb/QuaTrinhDaoTaoCuaCanBo");
            return tbQuaTrinhDaoTaoCuaCanBos.Any(e => e.IdQuaTrinhDaoTaoCuaCanBo == id);
        }


        //Import Excel 
        //Import Excel 
        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                List<TbQuaTrinhDaoTaoCuaCanBo> lst = new List<TbQuaTrinhDaoTaoCuaCanBo>();
            

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbQuaTrinhDaoTaoCuaCanBo> tbQuaTrinhDaoTaoCuaCanBos = await ApiServices_.GetAll<TbQuaTrinhDaoTaoCuaCanBo>("/api/cb/QuaTrinhDaoTaoCuaCanBo");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbQuaTrinhDaoTaoCuaCanBo model = new TbQuaTrinhDaoTaoCuaCanBo();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdQuaTrinhDaoTaoCuaCanBo == id) || tbQuaTrinhDaoTaoCuaCanBos.Any(t => t.IdQuaTrinhDaoTaoCuaCanBo == id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdQuaTrinhDaoTaoCuaCanBo = id; // Gán ID
                    model.CoSoDaoTao = item[0];
                    model.ThoiGianBatDau = DateOnly.ParseExact(item[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.IdCanBo = ParseInt(item[2]);
                    model.IdNganhDaoTao = ParseInt(item[3]);
                    model.IdTrinhDoDaoTao = ParseInt(item[4]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbQuaTrinhDaoTaoCuaCanBo(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbQuaTrinhDaoTaoCuaCanBo(TbQuaTrinhDaoTaoCuaCanBo item)
        {
            await ApiServices_.Create<TbQuaTrinhDaoTaoCuaCanBo>("/api/cb/QuaTrinhDaoTaoCuaCanBo", item);
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
