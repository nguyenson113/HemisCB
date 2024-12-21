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
    // Controller quản lý quá trình công tác của cán bộ
    public class QuaTrinhCongTacCuaCanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        // Constructor nhận vào HemisContext để tương tác với cơ sở dữ liệu
        public QuaTrinhCongTacCuaCanBoController(ApiServices services)
        {
            //Đọc dữ liệu từ APIService
            ApiServices_ = services;
        }

        public async Task<IActionResult> Statistics()
        {
            List<TbQuaTrinhCongTacCuaCanBo> getall = await TbQuaTrinhCongTacCuaCanBos();
            return View(getall);
        }



        //============================TẠO DANH SÁCH LẤY API========================
        private async Task<List<TbQuaTrinhCongTacCuaCanBo>> TbQuaTrinhCongTacCuaCanBos()
        {
            List<TbQuaTrinhCongTacCuaCanBo> tbQuaTrinhCongTacCuaCanBos = await ApiServices_.GetAll<TbQuaTrinhCongTacCuaCanBo>("/api/cb/QuaTrinhCongTacCuaCanBo");
            List<DmChucDanhGiangVien> dmchucDanhGiangViens = await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien");
            List<DmChucVu> dmchucVus = await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbQuaTrinhCongTacCuaCanBos.ForEach(item => {
                item.IdChucDanhGiangVienNavigation = dmchucDanhGiangViens.FirstOrDefault(x => x.IdChucDanhGiangVien == item.IdChucDanhGiangVien);
                item.IdChucVuNavigation = dmchucVus.FirstOrDefault(x => x.IdChucVu == item.IdChucVu);
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbQuaTrinhCongTacCuaCanBos;
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

        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbQuaTrinhCongTacCuaCanBo> getall = await TbQuaTrinhCongTacCuaCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        

      
        // GET: QuaTrinhCongTacCuaCanBo/Details/5
        // Phương thức hiển thị chi tiết một quá trình công tác
        public async Task<IActionResult> Details(int? id)
        {
            // Kiểm tra nếu id là null
            if (id == null)
            {
                return NotFound();
            }

            // Tìm quá trình công tác theo id
            var tbQuaTrinhCongTacCuaCanBos = await TbQuaTrinhCongTacCuaCanBos();
            var tbQuaTrinhCongTacCuaCanBo = tbQuaTrinhCongTacCuaCanBos.FirstOrDefault(m => m.IdQuaTrinhCongTacCuaCanBo == id);

            // Kiểm tra nếu không tìm thấy
            if (tbQuaTrinhCongTacCuaCanBo == null)
            {
                return NotFound();
            }

            // Trả về view với thông tin chi tiết
            return View(tbQuaTrinhCongTacCuaCanBo);
        }


        // GET: QuaTrinhCongTacCuaCanBo/Create
        // Phương thức hiển thị form tạo mới quá trình công tác
        public async Task<IActionResult> Create()
        {
            // Tạo danh sách lựa chọn cho IdCanBo, IdChucDanhGiangVien, IdChucVu
            ViewData["IdChucDanhGiangVien"] = new SelectList(await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien"), "IdChucDanhGiangVien", "ChucDanhGiangVien");
            ViewData["IdChucVu"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "ChucVu");
            ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
            return View();
        }

        // POST: QuaTrinhCongTacCuaCanBo/Create
        // Phương thức xử lý việc tạo mới quá trình công tác
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdQuaTrinhCongTacCuaCanBo,IdCanBo,TuThangNam,DenThangNam,IdChucVu,IdChucDanhGiangVien,DonViCongTac")] TbQuaTrinhCongTacCuaCanBo tbQuaTrinhCongTacCuaCanBo)
        {
            // Kiểm tra tính hợp lệ của model
            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Create<TbQuaTrinhCongTacCuaCanBo>("/api/cb/QuaTrinhCongTacCuaCanBo", tbQuaTrinhCongTacCuaCanBo);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //ViewBag.ErrorMessage = ex.Message;
                    ViewBag.ErrorMessage = ex.Message + " Đây là lỗi người làm ra chưa tìm ra cách fix thông cảm";
                }
                //finally
                //{
                //    ViewBag.mess = "Vui lòng nhập lại";
                //}
            }

            // Nếu không hợp lệ, tạo lại danh sách lựa chọn
            ViewData["IdChucDanhGiangVien"] = new SelectList(await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien"), "IdChucDanhGiangVien", "ChucDanhGiangVien", tbQuaTrinhCongTacCuaCanBo.IdChucDanhGiangVien);
            ViewData["IdChucVu"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "ChucVu", tbQuaTrinhCongTacCuaCanBo.IdChucVu);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbQuaTrinhCongTacCuaCanBo.IdCanBo);
           
          
            return View(tbQuaTrinhCongTacCuaCanBo); // Trả về view với thông tin đã nhập
        }
    


        // GET: QuaTrinhCongTacCuaCanBo/Edit/5
        // Phương thức hiển thị form chỉnh sửa quá trình công tác
        public async Task<IActionResult> Edit(int? id)
        {
            // Kiểm tra nếu id là null
            if (id == null)
            {
                return NotFound();
            }

            // Tìm quá trình công tác theo id
            var tbQuaTrinhCongTacCuaCanBo = await ApiServices_.GetId<TbQuaTrinhCongTacCuaCanBo>("/api/cb/QuaTrinhCongTacCuaCanBo", id ?? 0);
            if (tbQuaTrinhCongTacCuaCanBo == null)
            {
                return NotFound();
            }

            // Tạo danh sách lựa chọn cho IdCanBo, IdChucDanhGiangVien, IdChucVu
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbQuaTrinhCongTacCuaCanBo.IdCanBo);
            ViewData["IdChucDanhGiangVien"] = new SelectList(await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien"), "IdChucDanhGiangVien", "ChucDanhGiangVien", tbQuaTrinhCongTacCuaCanBo.IdChucDanhGiangVien);
            ViewData["IdChucVu"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "ChucVu", tbQuaTrinhCongTacCuaCanBo.IdChucVu);
            return View(tbQuaTrinhCongTacCuaCanBo); // Trả về view với thông tin để chỉnh sửa
        }

        // POST: QuaTrinhCongTacCuaCanBo/Edit/5
        // Phương thức xử lý việc chỉnh sửa quá trình công tác
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdQuaTrinhCongTacCuaCanBo,IdCanBo,TuThangNam,DenThangNam,IdChucVu,IdChucDanhGiangVien,DonViCongTac")] TbQuaTrinhCongTacCuaCanBo tbQuaTrinhCongTacCuaCanBo)
        {
            // Kiểm tra nếu id không khớp với thông tin đã chỉnh sửa
            if (id != tbQuaTrinhCongTacCuaCanBo.IdQuaTrinhCongTacCuaCanBo)
            {
                return NotFound();
            }

            // Kiểm tra tính hợp lệ của model
            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbQuaTrinhCongTacCuaCanBo>("/api/cb/QuaTrinhCongTacCuaCanBo", id, tbQuaTrinhCongTacCuaCanBo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Kiểm tra nếu quá trình công tác không tồn tại
                    if (await TbQuaTrinhCongTacCuaCanBoExists(tbQuaTrinhCongTacCuaCanBo.IdQuaTrinhCongTacCuaCanBo) == false)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Ném lỗi nếu có ngoại lệ
                    }
                }
                return RedirectToAction(nameof(Index)); // Chuyển hướng về danh sách
            }
            // Nếu không hợp lệ, tạo lại danh sách lựa chọn
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbQuaTrinhCongTacCuaCanBo.IdCanBo);
            ViewData["IdChucDanhGiangVien"] = new SelectList(await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien"), "IdChucDanhGiangVien", "ChucDanhGiangVien", tbQuaTrinhCongTacCuaCanBo.IdChucDanhGiangVien);
            ViewData["IdChucVu"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "ChucVu", tbQuaTrinhCongTacCuaCanBo.IdChucVu);
            return View(tbQuaTrinhCongTacCuaCanBo); // Trả về view với thông tin đã chỉnh sửa
        }
        #region xoá
        // GET: QuaTrinhCongTacCuaCanBo/Delete/5
        // Phương thức hiển thị form xác nhận xóa quá trình công tác
        public async Task<IActionResult> Delete(int? id)
        {
            // Kiểm tra nếu id là null
            if (id == null)
            {
                return NotFound();
            }

            var tbQuaTrinhCongTacCuaCanBos = await ApiServices_.GetAll<TbQuaTrinhCongTacCuaCanBo>("/api/cb/QuaTrinhCongTacCuaCanBo");
            var tbQuaTrinhCongTacCuaCanBo = tbQuaTrinhCongTacCuaCanBos.FirstOrDefault(m => m.IdQuaTrinhCongTacCuaCanBo == id);
            if (tbQuaTrinhCongTacCuaCanBo == null)
            {
                return NotFound();
            }

            // Trả về view với thông tin để xác nhận xóa
            return View(tbQuaTrinhCongTacCuaCanBo);
        }

        // POST: QuaTrinhCongTacCuaCanBo/Delete/5
        // Phương thức xử lý việc xóa quá trình công tác
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Tìm quá trình công tác theo id
            try
            {
                await ApiServices_.Delete<TbQuaTrinhCongTacCuaCanBo>("/api/cb/QuaTrinhCongTacCuaCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        #endregion

        // Phương thức kiểm tra xem quá trình công tác có tồn tại hay không
        private async Task<bool> TbQuaTrinhCongTacCuaCanBoExists(int id)
        {
            var tbQuaTrinhCongTacCuaCanBos = await ApiServices_.GetAll<TbQuaTrinhCongTacCuaCanBo>("/api/cb/QuaTrinhCongTacCuaCanBo");
            return tbQuaTrinhCongTacCuaCanBos.Any(e => e.IdQuaTrinhCongTacCuaCanBo == id);
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
