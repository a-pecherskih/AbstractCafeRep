using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

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
            List<MenuViewModel> result = source.Menus
                .Select(rec => new MenuViewModel
                {
                    Id = rec.Id,
                    MenuName = rec.MenuName,
                    Price = rec.Price,
                    MenuDishs = source.MenuDishs
                            .Where(recPC => recPC.MenuId == rec.Id)
                            .Select(recPC => new MenuDishViewModel
                            {
                                Id = recPC.Id,
                                MenuId = recPC.MenuId,
                                DishId = recPC.DishId,
                                DishName = source.Dishs
                                    .FirstOrDefault(recC => recC.Id == recPC.DishId)?.DishName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public MenuViewModel GetElement(int id)
        {
            Menu element = source.Menus.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new MenuViewModel
                {
                    Id = element.Id,
                    MenuName = element.MenuName,
                    Price = element.Price,
                    MenuDishs = source.MenuDishs
                            .Where(recCC => recCC.MenuId == element.Id)
                            .Select(recCC => new MenuDishViewModel
                            {
                                Id = recCC.Id,
                                MenuId = recCC.MenuId,
                                DishId = recCC.DishId,
                                DishName = source.Dishs
                                        .FirstOrDefault(recC => recC.Id == recCC.DishId)?.DishName,
                                Count = recCC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(MenuBindingModel model)
        {
            Menu element = source.Menus.FirstOrDefault(rec => rec.MenuName == model.MenuName);
            if (element != null)
            {
                throw new Exception("Уже есть меню с таким названием");
            }
            int maxId = source.Menus.Count > 0 ? source.Menus.Max(rec => rec.Id) : 0;
            source.Menus.Add(new Menu
            {
                Id = maxId + 1,
                MenuName = model.MenuName,
                Price = model.Price
            });

            int maxCCId = source.MenuDishs.Count > 0 ?
                                    source.MenuDishs.Max(rec => rec.Id) : 0;

            var groupDishs = model.MenuDishs
                                        .GroupBy(rec => rec.DishId)
                                        .Select(rec => new
                                        {
                                            DishId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });

            foreach (var groupDish in groupDishs)
            {
                source.MenuDishs.Add(new MenuDish
                {
                    Id = ++maxCCId,
                    MenuId = maxId + 1,
                    DishId = groupDish.DishId,
                    Count = groupDish.Count
                });
            }
        }

        public void UpdElement(MenuBindingModel model)
        {
            Menu element = source.Menus.FirstOrDefault(rec =>
                                        rec.MenuName == model.MenuName && rec.Id != model.Id);
            if (element != null)
            {
                throw new Exception("Уже есть меню с таким названием");
            }
            element = source.Menus.FirstOrDefault(rec => rec.Id == model.Id);
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            element.MenuName = model.MenuName;
            element.Price = model.Price;

            int maxPCId = source.MenuDishs.Count > 0 ? source.MenuDishs.Max(rec => rec.Id) : 0;

            var compIds = model.MenuDishs.Select(rec => rec.DishId).Distinct();
            var updateDishs = source.MenuDishs
                                            .Where(rec => rec.MenuId == model.Id &&
                                           compIds.Contains(rec.DishId));
            foreach (var updateDish in updateDishs)
            {
                updateDish.Count = model.MenuDishs
                                                .FirstOrDefault(rec => rec.Id == updateDish.Id).Count;
            }
            source.MenuDishs.RemoveAll(rec => rec.MenuId == model.Id &&
                                       !compIds.Contains(rec.DishId));

            var groupDishs = model.MenuDishs
                                        .Where(rec => rec.Id == 0)
                                        .GroupBy(rec => rec.DishId)
                                        .Select(rec => new
                                        {
                                            DishId = rec.Key,
                                            Count = rec.Sum(r => r.Count)
                                        });
            foreach (var groupDish in groupDishs)
            {
                MenuDish elementCC = source.MenuDishs
                                        .FirstOrDefault(rec => rec.MenuId == model.Id &&
                                                        rec.DishId == groupDish.DishId);
                if (elementCC != null)
                {
                    elementCC.Count += groupDish.Count;
                }
                else
                {
                    source.MenuDishs.Add(new MenuDish
                    {
                        Id = ++maxPCId,
                        MenuId = model.Id,
                        DishId = groupDish.DishId,
                        Count = groupDish.Count
                    });
                }
            }
        }

        public void DelElement(int id)
        {
            Menu element = source.Menus.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                source.MenuDishs.RemoveAll(rec => rec.MenuId == id);
                source.Menus.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }
    }
}
