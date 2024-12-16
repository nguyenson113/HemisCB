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

namespace HemisCB.Controllers.CB
{
    public class LinhVucNghienCuuCuaCanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        public LinhVucNghienCuuCuaCanBoController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //==========================================TẠO DANH SÁCH THÔNG TIN TỪ API===========================
        private async Task<List<TbLinhVucNghienCuuCuaCanBo>> TbLinhVucNghienCuuCuaCanBos()
        {
            List<TbLinhVucNghienCuuCuaCanBo> tbLinhVucNghienCuuCuaCanBos = await ApiServices_.GetAll<TbLinhVucNghienCuuCuaCanBo>("/api/cb/LinhVucNghienCuuCuaCanBo");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmLinhVucNghienCuu> dmlinhVucNghienCuus = await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu");
            tbLinhVucNghienCuuCuaCanBos.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdLinhVucNghienCuuNavigation = dmlinhVucNghienCuus.FirstOrDefault(x => x.IdLinhVucNghienCuu == item.IdLinhVucNghienCuu);
            });
            return tbLinhVucNghienCuuCuaCanBos;
        }

        public async Task<IActionResult> Statistics()
        {
            List<TbLinhVucNghienCuuCuaCanBo> getall = await TbLinhVucNghienCuuCuaCanBos();
            return View(getall);
        }



        // GET: LinhVucNghienCuuCuaCanBo
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbLinhVucNghienCuuCuaCanBo> getall = await TbLinhVucNghienCuuCuaCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        // GET: LinhVucNghienCuuCuaCanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbLinhVucNghienCuuCuaCanBos = await TbLinhVucNghienCuuCuaCanBos();
                var tbLinhVucNghienCuuCuaCanBo = tbLinhVucNghienCuuCuaCanBos.FirstOrDefault(m => m.IdLinhVucNghienCuuCuaCanBo == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbLinhVucNghienCuuCuaCanBo == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbLinhVucNghienCuuCuaCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        /// <summary>
        /// Hàm khởi tạo dữ liệu lĩnh vực nghiên cứu của cán bộ 
        /// </summary>
        /// <returns></returns>
      
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo");
                ViewData["IdLinhVucNghienCuu"] = new SelectList(await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu"), "IdLinhVucNghienCuu", "LinhVucNghienCuu");
               
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }


        // POST: LinhVucNghienCuuCuaCanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLinhVucNghienCuuCuaCanBo,IdCanBo,IdLinhVucNghienCuu,LaLinhVucNghienCuuChuyenSau,SoNamNghienCuu")] TbLinhVucNghienCuuCuaCanBo tbLinhVucNghienCuuCuaCanBo)
        {
            // Kiểm tra dữ liệu có chuẩn không
            // Đối sánh với lớp tbLinhVucNghienCuuCuaCanBo

            if (await TbLinhVucNghienCuuCuaCanBoExists(tbLinhVucNghienCuuCuaCanBo.IdLinhVucNghienCuuCuaCanBo)) ModelState.AddModelError("IdLinhVucNghienCuuCuaCanBo", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbLinhVucNghienCuuCuaCanBo>("/api/cb/LinhVucNghienCuuCuaCanBo", tbLinhVucNghienCuuCuaCanBo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbLinhVucNghienCuuCuaCanBo.IdCanBo);
            ViewData["IdLinhVucNghienCuu"] = new SelectList(await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu"), "IdLinhVucNghienCuu", "LinhVucNghienCuu", tbLinhVucNghienCuuCuaCanBo.IdLinhVucNghienCuu);
            return View(tbLinhVucNghienCuuCuaCanBo);
        }
            
        /// <summary>
        /// Khởi tạo thông tin sửa dữ liệu nghiên cứu của cán bộ
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Views tạo</returns>
        // GET: LinhVucNghienCuuCuaCanBo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbLinhVucNghienCuuCuaCanBo = await ApiServices_.GetId<TbLinhVucNghienCuuCuaCanBo>("/api/cb/LinhVucNghienCuuCuaCanBo", id ?? 0);
            if (tbLinhVucNghienCuuCuaCanBo == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbLinhVucNghienCuuCuaCanBo.IdCanBo);
            ViewData["IdLinhVucNghienCuu"] = new SelectList(await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu"), "IdLinhVucNghienCuu", "LinhVucNghienCuu", tbLinhVucNghienCuuCuaCanBo.IdLinhVucNghienCuu);
            return View(tbLinhVucNghienCuuCuaCanBo);
        }

        // POST: LinhVucNghienCuuCuaCanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdLinhVucNghienCuuCuaCanBo,IdCanBo,IdLinhVucNghienCuu,LaLinhVucNghienCuuChuyenSau,SoNamNghienCuu")] TbLinhVucNghienCuuCuaCanBo tbLinhVucNghienCuuCuaCanBo)
        {
            if (id != tbLinhVucNghienCuuCuaCanBo.IdLinhVucNghienCuuCuaCanBo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbLinhVucNghienCuuCuaCanBo>("/api/cb/LinhVucNghienCuuCuaCanBo", id, tbLinhVucNghienCuuCuaCanBo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbLinhVucNghienCuuCuaCanBoExists(tbLinhVucNghienCuuCuaCanBo.IdLinhVucNghienCuuCuaCanBo) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbLinhVucNghienCuuCuaCanBo.IdCanBo);
            ViewData["IdLinhVucNghienCuu"] = new SelectList(await ApiServices_.GetAll<DmLinhVucNghienCuu>("/api/dm/LinhVucNghienCuu"), "IdLinhVucNghienCuu", "LinhVucNghienCuu", tbLinhVucNghienCuuCuaCanBo.IdLinhVucNghienCuu);
            return View(tbLinhVucNghienCuuCuaCanBo);
        }

        // GET: LinhVucNghienCuuCuaCanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbLinhVucNghienCuuCuaCanBos = await ApiServices_.GetAll<TbLinhVucNghienCuuCuaCanBo>("/api/cb/LinhVucNghienCuuCuaCanBo");
            var tbLinhVucNghienCuuCuaCanBo = tbLinhVucNghienCuuCuaCanBos.FirstOrDefault(m => m.IdLinhVucNghienCuuCuaCanBo == id);
            if (tbLinhVucNghienCuuCuaCanBo == null)
            {
                return NotFound();
            }

            return View(tbLinhVucNghienCuuCuaCanBo);
        }


        // POST: LinhVucNghienCuuCuaCanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbLinhVucNghienCuuCuaCanBo>("/api/cb/LinhVucNghienCuuCuaCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbLinhVucNghienCuuCuaCanBoExists(int id)
        {
            var tbLinhVucNghienCuuCuaCanBos = await ApiServices_.GetAll<TbLinhVucNghienCuuCuaCanBo>("/api/cb/LinhVucNghienCuuCuaCanBo");
            return tbLinhVucNghienCuuCuaCanBos.Any(e => e.IdLinhVucNghienCuuCuaCanBo == id);
        }


        //Import Excel 
        public IActionResult Excel(string json)
        {
            try
            {
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);
                Console.WriteLine(JsonConvert.SerializeObject(data));
                return Accepted(Json(new { msg = JsonConvert.SerializeObject(data) }));
            }
            catch (Exception ex)
            {
                return BadRequest(Json(new { msg = "Lỗi nè mấy má !!!!!!!!" }));
            }

        }
    }
}
