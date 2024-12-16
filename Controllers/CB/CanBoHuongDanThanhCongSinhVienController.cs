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
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;

namespace HemisCB.Controllers.CB
{
    public class CanBoHuongDanThanhCongSinhVienController : Controller
    {
        private readonly ApiServices ApiServices_;

        public CanBoHuongDanThanhCongSinhVienController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //================================== TẠO LIST LẤY DỮ LIỆU CHO BẢNG TỪ APIHemis  ==============================
        private async Task<List<TbCanBoHuongDanThanhCongSinhVien>> TbCanBoHuongDanThanhCongSinhViens()
        {
                //Tạo bảng TbCanBoHuongDanThanhCongSinhVien lấy dữu liệu từ APIHemis /api/cb/CanBoHuongDanThanhCongSinhVien 
                List<TbCanBoHuongDanThanhCongSinhVien> tbCanBoHuongDanThanhCongSinhViens = await ApiServices_.GetAll<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien");
                //Lấy dữ liệu liên qua trong TbCanBo từ APIHemis /api/cb/CanBo 
                List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
                tbCanBoHuongDanThanhCongSinhViens.ForEach(item =>
                {
                    //Lấy dữ liệu cho IdCanBoNavigation 
                    item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                });
                //Trả kết quả 
                return tbCanBoHuongDanThanhCongSinhViens;
           
            
        }
        public async Task<IActionResult> Statistics()
        {
            List<TbCanBoHuongDanThanhCongSinhVien> getall = await TbCanBoHuongDanThanhCongSinhViens();
            return View(getall);
        }


        //================================================================================ INDEX ==============================================================================
        // GET: CanBoHuongDanThanhCongSinhVien
        //Trang Index của bảng CanBoHuongDanThanhCongSinhVien 
        public async Task<IActionResult> Index()
        {
            //Tạo khối try - catch để bắt lỗi 
            try
            {
                //Tạo bảng và lấy tất cả các dữ liệu đã khai báo trong danh sách dữ liệu API đã tạo 
                List<TbCanBoHuongDanThanhCongSinhVien> getall = await TbCanBoHuongDanThanhCongSinhViens();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        //======================================================================== DETAIL =====================================================================================
        // GET: CanBoHuongDanThanhCongSinhVien/Details/5
        //Trang Detail để xem chi tiết thông tin của bảng CanBoHuongDanThanhCongSinhVien 
        public async Task<IActionResult> Details(int? id)
        {
            //Tạo khối try - catch trong Detail để bắt lỗi nếu có và trả về kết quả HTTp Error 400 
            try
            {
                //Kiểm tra id nhập vào nếu không có thì trả về NotFound
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbCanBoHuongDanThanhCongSinhViens = await TbCanBoHuongDanThanhCongSinhViens();
                var tbCanBoHuongDanThanhCongSinhVien = tbCanBoHuongDanThanhCongSinhViens.FirstOrDefault(m => m.IdCanBoHuongDanThanhCongSinhVien == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbCanBoHuongDanThanhCongSinhVien == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbCanBoHuongDanThanhCongSinhVien);
            }
            catch (Exception ex)
            {
                //Trả về HTTP Error 400 
                return BadRequest();
            }

        }

        //============================================================================= CREATE ===================================================================
        // GET: CanBoHuongDanThanhCongSinhVien/Create
        //Trang Create để tạo thông tin của bảng CanBoHuongDanThanhCongSinhVien 
        public async Task<IActionResult> Create()
        {
            //Tạo khối try - catch trong Create để bắt lỗi nếu có và trả về kết quả HTTp Error 400
            try
            {
                //Tạo SelectList của IdCanBo 
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo");
                return View();
            }
            //Bắt lỗi nếu có và lưu lỗi vào BatLoi
            catch (Exception BatLoi)
            {
                //Trả về lỗi HTTP Error 400 
                return BadRequest();
            }

        }


        // POST: CanBoHuongDanThanhCongSinhVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCanBoHuongDanThanhCongSinhVien,IdCanBo,IdSinhVien,TrachNhiemHuongDan,ThoiGianBatDau,ThoiGianKetThuc")] TbCanBoHuongDanThanhCongSinhVien tbCanBoHuongDanThanhCongSinhVien)
        {
            //Tạo khối try - catch trong Create để bắt lỗi nếu có và trả về kết quả HTTp Error 400
            try
            {
                //Kiểm tra nếu IdCanBoHuongDanThanhCongSinhVien nếu bị trùng thì báo lỗi "ID này đã tồn tại!"
                if (await TbCanBoHuongDanThanhCongSinhVienExists(tbCanBoHuongDanThanhCongSinhVien.IdCanBoHuongDanThanhCongSinhVien)) ModelState.AddModelError("IdCanBoHuongDanThanhCongSinhVien", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien", tbCanBoHuongDanThanhCongSinhVien);
                    return RedirectToAction(nameof(Index));
                }
                //Hiển thị SelectList của IdCanBo, chọn và lưu dữ liệu vào biến IdCanBo 
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbCanBoHuongDanThanhCongSinhVien.IdCanBo);
                return View(tbCanBoHuongDanThanhCongSinhVien);
            }catch(Exception BatLoi)   //Bắt lỗi nếu có và lưu lỗi vào BatLoi
            {
                //Trả về lỗi HTTP Error 400 
                return BadRequest();
            }
        }

        //======================================================================== EDIT ====================================================================
        // GET: CanBoHuongDanThanhCongSinhVien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //Tạo khối try - catch trong Create để bắt lỗi nếu có và trả về kết quả HTTp Error 400
            try
            {
                //Kiểm tra nếu IdCanBoHuongDanThanhCongSinhVien nếu bị trùng thì báo lỗi "ID này đã tồn tại!"
                if (id == null)
                {
                    return NotFound();
                }
                //Truy cập vào thông tin đã nhập dựa vào IdCanBoHuongDanThanhCongSinhVien 
                var tbCanBoHuongDanThanhCongSinhVien = await ApiServices_.GetId<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien", id ?? 0);
                
                if (tbCanBoHuongDanThanhCongSinhVien == null)
                {
                    return NotFound();
                }
                //Hiển thị SelectList của IdCanBo, chọn và lưu dữ liệu vào biến IdCanBo 
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbCanBoHuongDanThanhCongSinhVien.IdCanBo);
                return View(tbCanBoHuongDanThanhCongSinhVien);
            }
            catch (Exception BatLoi)//Bắt lỗi nếu có và lưu lỗi vào BatLoi
            {
                //Trả về lỗi HTTP Error 400 
                return BadRequest();
            }
        }

        // POST: CanBoHuongDanThanhCongSinhVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCanBoHuongDanThanhCongSinhVien,IdCanBo,IdSinhVien,TrachNhiemHuongDan,ThoiGianBatDau,ThoiGianKetThuc")] TbCanBoHuongDanThanhCongSinhVien tbCanBoHuongDanThanhCongSinhVien)
        {
            if (id != tbCanBoHuongDanThanhCongSinhVien.IdCanBoHuongDanThanhCongSinhVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                try
                {
                    //Cập nhật lại dữ liệu đã chỉnh sửa vào bảng TbCanBoHuongDanThanhCongSinhVien 
                    await ApiServices_.Update<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien", id, tbCanBoHuongDanThanhCongSinhVien);

                }
                catch (DbUpdateConcurrencyException)
                {
                   
                    if (await TbCanBoHuongDanThanhCongSinhVienExists(tbCanBoHuongDanThanhCongSinhVien.IdCanBoHuongDanThanhCongSinhVien) == false)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //Trả về trang Index 
                return RedirectToAction(nameof(Index));
            }
            //Hiển thị SelectList của IdCanBo, chọn và lưu dữ liệu vào biến IdCanBo 
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbCanBoHuongDanThanhCongSinhVien.IdCanBo);
            return View(tbCanBoHuongDanThanhCongSinhVien);
        }

        //==================================================================== DELETE  ====================================================================
        // GET: CanBoHuongDanThanhCongSinhVien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //Tạo khối try - catch trong Delete để bắt lỗi nếu có và trả về kết quả HTTp Error 400
            try
            {
                //Kiểm tra Id nếu không tồn tại sẽ trả về NotFound()
                if (id == null)
                {
                    return NotFound();
                }
                var tbCanBoHuongDanThanhCongSinhViens = await ApiServices_.GetAll<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien");
                var tbCanBoHuongDanThanhCongSinhVien = tbCanBoHuongDanThanhCongSinhViens.FirstOrDefault(m => m.IdCanBoHuongDanThanhCongSinhVien == id);
                if (tbCanBoHuongDanThanhCongSinhVien == null)
                {
                    return NotFound();
                }

                return View(tbCanBoHuongDanThanhCongSinhVien);
            }
            catch (Exception BatLoi)//Bắt lỗi nếu có và lưu lỗi vào BatLoi
            {
                //Trả về lỗi HTTP Error 400 
                return BadRequest();
            }
        }

        // POST: CanBoHuongDanThanhCongSinhVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //Tạo khối try - catch trong Delete để bắt lỗi nếu có và trả về kết quả HTTp Error 400
            try
            {
                //Xóa dữ liệu thông qua IdCanBoHuongDanThanhCongSinhVien, sau khi xóa trả kết quả hiển thị về trang Index 
                await ApiServices_.Delete<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien", id);
                //Trả kết quả hiển thị về trang Index 
                return RedirectToAction(nameof(Index));
            }
            catch (Exception BatLoi)//Bắt lỗi nếu có và lưu lỗi vào BatLoi
            {
                //Trả về lỗi HTTP Error 400 
                return BadRequest();
            }

        }

        //Kiểm tra nếu nhập trùng IdCanBoHuongDanThanhCongSinhVien
        private async Task<bool> TbCanBoHuongDanThanhCongSinhVienExists(int id)
        {
            var tbCanBoHuongDanThanhCongSinhViens = await ApiServices_.GetAll<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien");
            return tbCanBoHuongDanThanhCongSinhViens.Any(e => e.IdCanBoHuongDanThanhCongSinhVien == id);
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
