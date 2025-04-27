using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace RestAPIDemo1
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly LabDBContext _context;
        public UserInfoController(LabDBContext context)
        {
            _context = context;
        }
        // GET: UserInfo
        // 查詢所有使用者資料 (Read)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfo>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        // 查詢單一使用者資料 (Read)
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfo>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
        // 新增使用者資料 (Create)
        [HttpPost]
        public async Task<ActionResult<UserInfo>> CreateUser(UserInfo userInfo)
        {
            _context.Users.Add(userInfo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = userInfo.ID }, userInfo);
        }
        // 更新使用者資料 (Update)
        [HttpPut("id")]
        public async Task<ActionResult<UserInfo>> UpdateUser(int id, UserInfo userInfo)
        {
            if (id != userInfo.ID)
            {
                return BadRequest();
            }
            _context.Entry(userInfo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(e => e.ID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        // 刪除使用者資料 (Delete)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
