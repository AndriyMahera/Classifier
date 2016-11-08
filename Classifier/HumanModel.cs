using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classifier
{
    class HumanModel
    {
        public int Length
        {
            get { using (ClassifierEntities db = new ClassifierEntities())
                {
                    return db.Humans.Count();
                }
            }
        }
        public string Insert(Human human)
        {
            try
            {
                ClassifierEntities db = new ClassifierEntities();
                db.Humans.Add(human);
                db.SaveChanges();
                return human.ID + "was successfully inserted";
            }
            catch (Exception e)
            {
                return "Error" + e;
            }

        }
        public string Update(int id, Human human)
        {
            try
            {
                ClassifierEntities db = new ClassifierEntities();
                Human h = db.Humans.Find(id);

                h.HOG = human.HOG;
                h.IsHuman = human.IsHuman;

                db.SaveChanges();
                return human.ID + "was successfully updated";

            }
            catch (Exception e)
            {
                return "Error" + e;
            }
        }
        public string Delete(int id)
        {
            try
            {
                ClassifierEntities db = new ClassifierEntities();
                Human h = db.Humans.Find(id);

                db.Humans.Attach(h);
                db.Humans.Remove(h);
                db.SaveChanges();

                return h.ID + "was successfully deleted";

            }
            catch (Exception e)
            {
                return "Error" + e;
            }
        }
        public Human GetByID(int id)
        {
            try
            {
                using (ClassifierEntities se = new ClassifierEntities())
                {
                    Human human = se.Humans.Find(id);
                    return human;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public string DeleteAll()
        {
            try
            {
                ClassifierEntities db = new ClassifierEntities();
                db.Humans.RemoveRange(db.Humans);
                db.SaveChanges();

                return  "All items were successfully deleted";

            }
            catch (Exception e)
            {
                return "Error" + e;
            }
        }
        public List<Human> GetAll()
        {
            using (ClassifierEntities db=new ClassifierEntities())
            {
                return db.Humans.ToList();
            }
        }
    }
}
