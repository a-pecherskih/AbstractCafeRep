using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;

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
            List<DishViewModel> result = new List<DishViewModel>();
            for (int i = 0; i < source.Dishs.Count; ++i)
            {
                result.Add(new DishViewModel
                {
                    Id = source.Dishs[i].Id,
                    DishName = source.Dishs[i].DishName
                });
            }
            return result;
        }

        public DishViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Dishs.Count; ++i)
            {
                if (source.Dishs[i].Id == id)
                {
                    return new DishViewModel
                    {
                        Id = source.Dishs[i].Id,
                        DishName = source.Dishs[i].DishName
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(DishBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Dishs.Count; ++i)
            {
                if (source.Dishs[i].Id > maxId)
                {
                    maxId = source.Dishs[i].Id;
                }
                if (source.Dishs[i].DishName == model.DishName)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            source.Dishs.Add(new Dish
            {
                Id = maxId + 1,
                DishName = model.DishName
            });
        }

        public void UpdElement(DishBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Dishs.Count; ++i)
            {
                if (source.Dishs[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Dishs[i].DishName == model.DishName &&
                    source.Dishs[i].Id != model.Id)
                {
                    throw new Exception("Уже есть компонент с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Dishs[index].DishName = model.DishName;
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.Dishs.Count; ++i)
            {
                if (source.Dishs[i].Id == id)
                {
                    source.Dishs.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
