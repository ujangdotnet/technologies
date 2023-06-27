using Microsoft.AspNetCore.Mvc;
using xpos319.Models;

namespace xpos319.Controllers
{
    public class FriendController : Controller
    {
        //data dummy
        private static List<Friend> friends = new List<Friend>(){
            new Friend(){Id=1,Name="Razwan",Address="Landan"},
            new Friend(){Id=2,Name="Rizwin",Address="Lindin"},
            new Friend(){Id=3,Name="Ruzwun",Address="Lundun"},

            };

        //default view
        public IActionResult Index()
        {
            return View(friends);
            //atau dengan Viewbag.[nama-bag] = [nama variabel list]
        }

        //create data
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Friend frien)
        {
            friends.Add(frien);
            return RedirectToAction("Index");
        }

        //edit data
        public IActionResult Edit(int id)
        {
            //memanggil data models
            Friend friend = friends.Find(a => a.Id == id);
            return View(friend);
        }

        [HttpPost]
        public IActionResult Edit(Friend data)
        {
            Friend frien = friends.Find(a => a.Id == data.Id);
            int index = friends.IndexOf(frien);

            if (index > -1)
            {
                friends[index].Name = data.Name;
                friends[index].Address = data.Address;
            }

            return RedirectToAction("Index");
        }

        //detail data (sudah auto [httpget])
        public IActionResult Detail(int id)
        {
            Friend frien = friends.Find(a => a.Id == id);
            return View(frien);
        }

        //delete data
  
        public IActionResult Delete(int id)
        {
            Friend frien = friends.Find(a => a.Id == id);
            return View(frien);
            //return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(Friend data)
        {
            Friend frien = friends.Find(a => a.Id == data.Id);
            friends.Remove(frien);

            return RedirectToAction("Index");
        }
    }
    
}
