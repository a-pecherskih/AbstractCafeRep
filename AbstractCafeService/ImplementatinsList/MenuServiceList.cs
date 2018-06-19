using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;

namespace AbstractCafeService.ImplementatinsList
{
    public class MenuServiceList : IMenuService
    {
        private DataListSingleton source;

        public MenuServiceList()
        {
            source = DataListSingleton.GetInstance();
        }

        public List<MenuViewModel> GetList()
        {
            List<MenuViewModel> result = new List<MenuViewModel>();
            for (int i = 0; i < source.Menus.Count; ++i)
            {
                List<MenuDishViewModel> menuDishs = new List<MenuDishViewModel>();
                for (int j = 0; j < source.MenuDishs.Count; ++j)
                {
                    if (source.MenuDishs[j].MenuId == source.Menus[i].Id)
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
                        menuDishs.Add(new MenuDishViewModel
                        {
                            Id = source.MenuDishs[j].Id,
                            MenuId = source.MenuDishs[j].MenuId,
                            DishId = source.MenuDishs[j].DishId,
                            DishName = dishName,
                            Count = source.MenuDishs[j].Count
                        });
                    }
                }
                result.Add(new MenuViewModel
                {
                    Id = source.Menus[i].Id,
                    MenuName = source.Menus[i].MenuName,
                    Price = source.Menus[i].Price,
                    MenuDishs = menuDishs
                });
            }
            return result;
        }

        public MenuViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Menus.Count; ++i)
            {
                List<MenuDishViewModel> menuDishs = new List<MenuDishViewModel>();
                for (int j = 0; j < source.MenuDishs.Count; ++j)
                {
                    if (source.MenuDishs[j].MenuId == source.Menus[i].Id)
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
                        menuDishs.Add(new MenuDishViewModel
                        {
                            Id = source.MenuDishs[j].Id,
                            MenuId = source.MenuDishs[j].MenuId,
                            DishId = source.MenuDishs[j].DishId,
                            DishName = dishName,
                            Count = source.MenuDishs[j].Count
                        });
                    }
                }
                if (source.Menus[i].Id == id)
                {
                    return new MenuViewModel
                    {
                        Id = source.Menus[i].Id,
                        MenuName = source.Menus[i].MenuName,
                        Price = source.Menus[i].Price,
                        MenuDishs = menuDishs
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void AddElement(MenuBindingModel model)
        {
            int maxId = 0;
            for (int i = 0; i < source.Menus.Count; ++i)
            {
                if (source.Menus[i].Id > maxId)
                {
                    maxId = source.Menus[i].Id;
                }
                if (source.Menus[i].MenuName == model.MenuName)
                {
                    throw new Exception("Уже есть меню с таким названием");
                }
            }
            source.Menus.Add(new Menu
            {
                Id = maxId + 1,
                MenuName = model.MenuName,
                Price = model.Price
            });
            int maxPCId = 0;
            for (int i = 0; i < source.MenuDishs.Count; ++i)
            {
                if (source.MenuDishs[i].Id > maxPCId)
                {
                    maxPCId = source.MenuDishs[i].Id;
                }
            }
            for (int i = 0; i < model.MenuDishs.Count; ++i)
            {
                for (int j = 1; j < model.MenuDishs.Count; ++j)
                {
                    if (model.MenuDishs[i].DishId ==
                        model.MenuDishs[j].DishId)
                    {
                        model.MenuDishs[i].Count +=
                            model.MenuDishs[j].Count;
                        model.MenuDishs.RemoveAt(j--);
                    }
                }
            }
            for (int i = 0; i < model.MenuDishs.Count; ++i)
            {
                source.MenuDishs.Add(new MenuDish
                {
                    Id = ++maxPCId,
                    MenuId = maxId + 1,
                    DishId = model.MenuDishs[i].DishId,
                    Count = model.MenuDishs[i].Count
                });
            }
        }

        public void UpdElement(MenuBindingModel model)
        {
            int index = -1;
            for (int i = 0; i < source.Menus.Count; ++i)
            {
                if (source.Menus[i].Id == model.Id)
                {
                    index = i;
                }
                if (source.Menus[i].MenuName == model.MenuName &&
                    source.Menus[i].Id != model.Id)
                {
                    throw new Exception("Уже есть изделие с таким названием");
                }
            }
            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }
            source.Menus[index].MenuName = model.MenuName;
            source.Menus[index].Price = model.Price;
            int maxPCId = 0;
            for (int i = 0; i < source.MenuDishs.Count; ++i)
            {
                if (source.MenuDishs[i].Id > maxPCId)
                {
                    maxPCId = source.MenuDishs[i].Id;
                }
            }
            for (int i = 0; i < source.MenuDishs.Count; ++i)
            {
                if (source.MenuDishs[i].MenuId == model.Id)
                {
                    bool flag = true;
                    for (int j = 0; j < model.MenuDishs.Count; ++j)
                    {
                        if (source.MenuDishs[i].Id == model.MenuDishs[j].Id)
                        {
                            source.MenuDishs[i].Count = model.MenuDishs[j].Count;
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                        source.MenuDishs.RemoveAt(i--);
                    }
                }
            }
            for (int i = 0; i < model.MenuDishs.Count; ++i)
            {
                if (model.MenuDishs[i].Id == 0)
                {
                    for (int j = 0; j < source.MenuDishs.Count; ++j)
                    {
                        if (source.MenuDishs[j].MenuId == model.Id &&
                            source.MenuDishs[j].DishId == model.MenuDishs[i].DishId)
                        {
                            source.MenuDishs[j].Count += model.MenuDishs[i].Count;
                            model.MenuDishs[i].Id = source.MenuDishs[j].Id;
                            break;
                        }
                    }
                    if (model.MenuDishs[i].Id == 0)
                    {
                        source.MenuDishs.Add(new MenuDish
                        {
                            Id = ++maxPCId,
                            MenuId = model.Id,
                            DishId = model.MenuDishs[i].DishId,
                            Count = model.MenuDishs[i].Count
                        });
                    }
                }
            }
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.MenuDishs.Count; ++i)
            {
                if (source.MenuDishs[i].MenuId == id)
                {
                    source.MenuDishs.RemoveAt(i--);
                }
            }
            for (int i = 0; i < source.Menus.Count; ++i)
            {
                if (source.Menus[i].Id == id)
                {
                    source.Menus.RemoveAt(i);
                    return;
                }
            }
            throw new Exception("Элемент не найден");
        }
    }
}
