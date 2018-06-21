using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractCafeService.ImplementatinsList
{
    public class DishServiceList : IDishService
    {
        private DataListSingleton source;

        public DishServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<DishViewModel> GetList()
        {
            List<DishViewModel> result = source.Dishs
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
            Dish element = source.Dishs.FirstOrDefault(rec => rec.Id == id);
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
            Dish element = source.Dishs.FirstOrDefault(rec => rec.DishName == model.DishName);
            if (element != null)
            {
                throw new Exception("Уже есть блюдо с таким названием");
            }
            int maxId = source.Dishs.Count > 0 ? source.Dishs.Max(rec => rec.Id) : 0;
            source.Dishs.Add(new Dish
            {
                Id = maxId + 1,
                DishName = model.DishName
            });
        }

        public void UpdElement(DishBindingModel model)
        {
            Dish element = source.Dishs.FirstOrDefault(rec =>
                                        rec.DishName == model.DishName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть блюдо с таким названием");
            }
            element = source.Dishs.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.DishName = model.DishName;
        }

        public void DelElement(int id)
        {
            Dish element = source.Dishs.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.Dishs.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
