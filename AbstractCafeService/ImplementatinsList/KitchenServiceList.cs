using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;

namespace AbstractCafeService.ImplementatinsList
{
    public class KitchenServiceList : IKitchenService
    {
        private DataListSingleton source;

        public KitchenServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<KitchenViewModel> GetList()
        {
            List<KitchenViewModel> result = new List<KitchenViewModel>();
            for (int i = 0; i < source.Kitchens.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<KitchenDishViewModel> KitchenDishs = new List<KitchenDishViewModel>();
                for (int j = 0; j < source.KitchenDishs.Count; ++j)
                {
                    if (source.KitchenDishs[j].KitchenId == source.Kitchens[i].Id)
                    {
                        string dishName = string.Empty;
                        for (int k = 0; k < source.Dishs.Count; ++k)
                        {
                            if (source.MenuDishs[j].DishId == source.Dishs[k].Id)
                            {
                                dishName = source.Dishs[k].DishName;
                                break;
                            }
                        }
                        KitchenDishs.Add(new KitchenDishViewModel
                        {
                            Id = source.KitchenDishs[j].Id,
                            KitchenId = source.KitchenDishs[j].KitchenId,
                            DishId = source.KitchenDishs[j].DishId,
                            DishName = dishName,
                            Count = source.KitchenDishs[j].Count
                        });
                    }
                }
                result.Add(new KitchenViewModel
                {
                    Id = source.Kitchens[i].Id,
                    KitchenName = source.Kitchens[i].KitchenName,
                    KitchenDishs = KitchenDishs
                });
            }
            return result;
        }

        public KitchenViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Kitchens.Count; ++i)
            {
                // требуется дополнительно получить список компонентов на складе и их количество
                List<KitchenDishViewModel> KitchenDishs = new List<KitchenDishViewModel>();
                for (int j = 0; j < source.KitchenDishs.Count; ++j)
                {
                    if (source.KitchenDishs[j].KitchenId == source.Kitchens[i].Id)
                    {
                        string dishName = string.Empty;
                        for (int k = 0; k < source.Dishs.Count; ++k)
                        {
                            if (source.MenuDishs[j].DishId == source.Dishs[k].Id)
                            {
                                dishName = source.Dishs[k].DishName;
                                break;
                            }
                        }
                        KitchenDishs.Add(new KitchenDishViewModel
                        {
                            Id = source.KitchenDishs[j].Id,
                            KitchenId = source.KitchenDishs[j].KitchenId,
                            DishId = source.KitchenDishs[j].DishId,
                            DishName = dishName,
                            Count = source.KitchenDishs[j].Count
                        });
                    }
                }
                if (source.Kitchens[i].Id == id)
                {
                    return new KitchenViewModel
                    {
                        Id = source.Kitchens[i].Id,
                        KitchenName = source.Kitchens[i].KitchenName,
                        KitchenDishs = KitchenDishs
                    };
                }
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(KitchenBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Kitchens.Count; ++i)
            {
                if (source.Kitchens[i].Id > maxId)
                {
                    maxId = source.Kitchens[i].Id;
                }
                if (source.Kitchens[i].KitchenName == model.KitchenName)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            source.Kitchens.Add(new Kitchen
            {
                Id = maxId + 1,
                KitchenName = model.KitchenName
            });
        }

        public void UpdElement(KitchenBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Kitchens.Count; ++i)
            {
                if (source.Kitchens[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Kitchens[i].KitchenName == model.KitchenName &&
                    source.Kitchens[i].Id != model.Id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Kitchens[index].KitchenName = model.KitchenName;
        }

        public void DelElement(int id)
        {
            // при удалении удаляем все записи о компонентах на удаляемом складе
            for (int i = 0; i < source.KitchenDishs.Count; ++i)
            {
                if (source.KitchenDishs[i].KitchenId == id)
                {
                    source.KitchenDishs.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Kitchens.Count; ++i)
            {
                if (source.Kitchens[i].Id == id)
                {
                    source.Kitchens.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
