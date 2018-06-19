using AbstractCafeModel;
using AbstractCafeService.BindingModel;
using AbstractCafeService.Interfaces;
using AbstractCafeService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AbstractCafeService.ImplementationsBD
{
    public class MenuServiceBD : IMenuService
    {
        private AbstractDbContext context;

        public MenuServiceBD(AbstractDbContext context)
        {
            this.context = context;
        }

        public List<MenuViewModel> GetList()
        {
            List<MenuViewModel> result = context.Menus
                .Select(rec => new MenuViewModel
                {
                    Id = rec.Id,
                    MenuName = rec.MenuName,
                    Price = rec.Price,
                    MenuDishs = context.MenuDishs
                            .Where(recPC => recPC.MenuId == rec.Id)
                            .Select(recPC => new MenuDishViewModel
                            {
                                Id = recPC.Id,
                                MenuId = recPC.MenuId,
                                DishId = recPC.DishId,
                                DishName = recPC.Dish.DishName,
                                Count = recPC.Count
                            })
                            .ToList()
                })
                .ToList();
            return result;
        }

        public MenuViewModel GetElement(int id)
        {
            Menu element = context.Menus.FirstOrDefault(rec => rec.Id == id);
            if (element != null)
            {
                return new MenuViewModel
                {
                    Id = element.Id,
                    MenuName = element.MenuName,
                    Price = element.Price,
                    MenuDishs = context.MenuDishs
                            .Where(recPC => recPC.MenuId == element.Id)
                            .Select(recPC => new MenuDishViewModel
                            {
                                Id = recPC.Id,
                                MenuId = recPC.MenuId,
                                DishId = recPC.DishId,
                                DishName = recPC.Dish.DishName,
                                Count = recPC.Count
                            })
                            .ToList()
                };
            }
            throw new Exception("Элемент не найден");
        }

        public void AddElement(MenuBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Menu element = context.Menus.FirstOrDefault(rec => rec.MenuName == model.MenuName);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = new Menu
                    {
                        MenuName = model.MenuName,
                        Price = model.Price
                    };
                    context.Menus.Add(element);
                    context.SaveChanges();
                    // убираем дубли по компонентам
                    var groupDishs = model.MenuDishs
                                                .GroupBy(rec => rec.DishId)
                                                .Select(rec => new
                                                {
                                                    DishId = rec.Key,
                                                    Count = rec.Sum(r => r.Count)
                                                });
                    // добавляем компоненты
                    foreach (var groupDish in groupDishs)
                    {
                        context.MenuDishs.Add(new MenuDish
                        {
                            MenuId = element.Id,
                            DishId = groupDish.DishId,
                            Count = groupDish.Count
                        });
                        context.SaveChanges();
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void UpdElement(MenuBindingModel model)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Menu element = context.Menus.FirstOrDefault(rec =>
                                        rec.MenuName == model.MenuName && rec.Id != model.Id);
                    if (element != null)
                    {
                        throw new Exception("Уже есть изделие с таким названием");
                    }
                    element = context.Menus.FirstOrDefault(rec => rec.Id == model.Id);
                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                    element.MenuName = model.MenuName;
                    element.Price = model.Price;
                    context.SaveChanges();

                    // обновляем существуюущие компоненты
                    var dishIds = model.MenuDishs.Select(rec => rec.DishId).Distinct();
                    var updateDishs = context.MenuDishs
                                                    .Where(rec => rec.MenuId == model.Id &&
                                                        dishIds.Contains(rec.DishId));
                    foreach (var updateDish in updateDishs)
                    {
                        updateDish.Count = model.MenuDishs
                                                        .FirstOrDefault(rec => rec.Id == updateDish.Id).Count;
                    }
                    context.SaveChanges();
                    context.MenuDishs.RemoveRange(
                                        context.MenuDishs.Where(rec => rec.MenuId == model.Id &&
                                                                            !dishIds.Contains(rec.DishId)));
                    context.SaveChanges();
                    // новые записи
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
                        MenuDish elementPC = context.MenuDishs
                                                .FirstOrDefault(rec => rec.MenuId == model.Id &&
                                                                rec.DishId == groupDish.DishId);
                        if (elementPC != null)
                        {
                            elementPC.Count += groupDish.Count;
                            context.SaveChanges();
                        }
                        else
                        {
                            context.MenuDishs.Add(new MenuDish
                            {
                                MenuId = model.Id,
                                DishId = groupDish.DishId,
                                Count = groupDish.Count
                            });
                            context.SaveChanges();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void DelElement(int id)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    Menu element = context.Menus.FirstOrDefault(rec => rec.Id == id);
                    if (element != null)
                    {
                        // удаяем записи по компонентам при удалении изделия
                        context.MenuDishs.RemoveRange(
                                            context.MenuDishs.Where(rec => rec.MenuId == id));
                        context.Menus.Remove(element);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Элемент не найден");
                    }
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
