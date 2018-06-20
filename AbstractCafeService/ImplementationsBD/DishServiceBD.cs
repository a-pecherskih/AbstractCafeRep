using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractCafeService.ImplementationsBD
{
    public class DishServiceBD : IDishService
    {
        private AbstractDbContext context;

        public DishServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<DishViewModel> GetList()
        {
            List<DishViewModel> result = context.Dishs
                .Select(rec => new DishViewModel
                {
                    Id = rec.Id,
                    DishName = rec.DishName
                })
                .ToList();
            return result;
        }

        public DishViewModel GetElement(int id)
        {
            Dish element = context.Dishs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new DishViewModel
                {
                    Id = element.Id,
                    DishName = element.DishName
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(DishBindingModel model)
        {
            Dish element = context.Dishs.FirstOrDefault(rec => rec.DishName == model.DishName);
            if (element != null)
            {
                throw new Exception("Уже есть блюдо с таким названием");
            }
            context.Dishs.Add(new Dish
            {
                DishName = model.DishName
            });
            context.SaveChanges();
        }

        public void UpdElement(DishBindingModel model)
        {
            Dish element = context.Dishs.FirstOrDefault(rec =>
                                        rec.DishName == model.DishName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть блюдо с таким названием");
            }
            element = context.Dishs.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.DishName = model.DishName;
            context.SaveChanges();
        }

        public void DelElement(int id)
        {
            Dish element = context.Dishs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                context.Dishs.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
