using System;
using System.Collections.Generic;
using Entities;
using System.Linq;
using BusinessLogicLayer;

namespace PresetationLayer
{
    public static class ConsoleUI
    {
        private const string StringSeparator = "=====================================";

        static BLL core = new BLL();

        public static void Run()
        {
            Console.Title = "Система управления рассылками";


            while (true)
            {
                Console.WriteLine("Добро пожаловать в систему управления рассылками");
                Console.WriteLine("Выберите действие: ");
                Console.WriteLine("1. Получить список всех рассылок\n" +
                                  "2. Получить список всех пользователей\n" +
                                  "3. Добавить пользователя\n" +
                                  "4. Добавить рассылку\n" +
                                  "5. Изменить название рассылки\n" +
                                  "6. Удалить список рассылки\n" +
                                  "7. Вывести списки рассылки, на которые подписан пользователь\n" +
                                  "8. Получить рассылки, которые подлежат удалению\n" +
                                  "9. Добавить пользователя в список\n\n" +
                                  "0. Выйти\n");
                string answer = Console.ReadLine();
                #region switch

                switch (answer)
                {
                    case "1":
                        {
                            PrintDistributeLists(core.GetDistributeLists());
                            Console.WriteLine("Нажмите <Enter> для возврата в главное меню");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        }
                    case "2":
                        {
                            PrintUsers(core.GetUsers());
                            Console.WriteLine("Нажмите <Enter> для возврата в главное меню");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        }
                    case "3":
                        {
                            //Method contains its console menu
                            core.AddUser(CreatingUser());
                            Console.WriteLine("Нажмите <Enter> для возврата в главное меню");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        }
                    case "4":
                        {
                            core.GetDistributeLists().Add(CreatingDistributeList());
                            Console.WriteLine("Нажмите <Enter> для возврата в главное меню");
                            Console.ReadLine();
                            Console.Clear();
                            break;
                        }
                    case "5":
                        {
                            ChangeTitleOfDistributeList();
                            break;
                        }
                    case "6":
                        {
                            DeleteOfDistributeList();
                            break;
                        }
                    case "7":
                        {
                            PrintSubscriptionOfUser();
                            break;
                        }
                    case "8":
                        {
                            PrintCandidatesForDeletion();
                            break;
                        }
                    case "9":
                        {
                            AddUserToList();
                            break;
                        }
                    case "0":
                        {
                            Console.WriteLine("Сохранить изменения? (y/n)\n");

                            string userAnswer = Console.ReadLine();

                            switch (userAnswer)
                            {
                                case "y": core.SaveChanges(); return;
                                case "n": return;
                                default: Console.Clear(); continue;
                            }
                        }
                    default:
                        {
                            Console.WriteLine("Некорректный ввод\nНажмите <Enter> для возврата в главное меню\n" + StringSeparator);
                            Console.ReadLine();
                            Console.Clear();
                            continue;
                        }
                }

                #endregion

            }

        }

        private static void PrintDistributeLists(List<DistributeList> distributeLists)
        {
            if (distributeLists.Count == 0)
            {
                Console.WriteLine("Список пуст!");
            }
            else
            {
                for (int i = 0; i < distributeLists.Count; i++)
                {
                    Console.WriteLine("\t{0}. Название: {1}\n\tОписание: {2}\n", i + 1, distributeLists[i].Title, distributeLists[i].Description);
                }
            }
        }

        private static void PrintUsers(List<User> users)
        {
            for (int i = 0; i < users.Count; i++)
            {
                var user = users[i];
                Console.WriteLine("\t{0}. {1} {2} {3} ({4})", i + 1, user.FirstName, user.LastName, user.MiddleName, user.Login);
            }

        }

        /// <summary>
        /// Opening dialog to users using console
        /// </summary>
        /// <returns>Created user</returns>
        private static User CreatingUser()
        {
            Console.WriteLine("Добавление нового пользователя");

            string login;
            Console.WriteLine("Придумайте логин пользователя");
            while (string.IsNullOrWhiteSpace(login = Console.ReadLine()))
            {
                Console.WriteLine("Это поле не может быть пустым");
                Console.WriteLine("Придумайте логин пользователя");
            }

            while (!core.GetUsers().TrueForAll(user => user.Login != login))
            {
                Console.WriteLine("Этот логин уже занят");
                Console.WriteLine("Придумайте логин пользователя");
            }

            Console.WriteLine("Введите имя");
            string firstName;
            while (string.IsNullOrWhiteSpace(firstName = Console.ReadLine()))
            {
                Console.WriteLine("Это поле не может быть пустым");
                Console.WriteLine("Введите имя");
            }

            Console.WriteLine("Введите фамилию");
            string lastName;
            while (string.IsNullOrWhiteSpace(lastName = Console.ReadLine()))
            {
                Console.WriteLine("Это поле не может быть пустым");
                Console.WriteLine("Введите фамилию");
            }

            Console.WriteLine("Введите отчество");
            string middleName;
            while (string.IsNullOrWhiteSpace(middleName = Console.ReadLine()))
            {
                Console.WriteLine("Это поле не может быть пустым");
                Console.WriteLine("Введите фамилию");
            }

            return new User(login, firstName, lastName, middleName);
        }

        /// <summary>
        /// Opening dialog to users using console
        /// </summary>
        private static DistributeList CreatingDistributeList()
        {
            Console.Clear();
            Console.WriteLine("Создание рассылки");
            Console.WriteLine("\nВведите название");
            string title;
            while (string.IsNullOrWhiteSpace(title = Console.ReadLine()))
            {
                Console.WriteLine("Это поле не может быть пустым");
                Console.WriteLine("\nВведите название");
            }
            Console.WriteLine("\nВведите описание рассылки");
            string description;
            while (string.IsNullOrWhiteSpace(description = Console.ReadLine()))
            {
                Console.WriteLine("Это поле не может быть пустым");
                Console.WriteLine("\nВведите название");
            }

            return new DistributeList(title, description);
        }

        /// <summary>
        /// Change title using console dialog
        /// </summary>
        private static void ChangeTitleOfDistributeList()
        {
            var lists = core.GetDistributeLists();
            int answer;

            Console.WriteLine("Выберите рассылку для изменения названия\n");
            PrintDistributeLists(lists);

            while (!int.TryParse(Console.ReadLine(), out answer) || answer < 1 || answer > lists.Count)
            {
                Console.WriteLine("Некорректный ввод. Попробуйте ещё раз\n");
            }

            Console.WriteLine("Введите новое название рассылки\n");
            string title;
            while (string.IsNullOrWhiteSpace(title = Console.ReadLine()))
            {
                Console.WriteLine("Некорректный ввод. Попробуйте ещё раз\n");
            }

            lists[answer - 1].Title = title;

            Console.WriteLine("Название успешно изменено!");

            Console.WriteLine("Нажмите <Enter> для возврата в главное меню");
            Console.ReadLine();
            Console.Clear();
        }

        /// <summary>
        /// Delete list using console dialog
        /// </summary>
        private static void DeleteOfDistributeList()
        {
            var lists = core.GetDistributeLists();
            int answer;

            Console.WriteLine("Выберите рассылку для удаления\n");
            PrintDistributeLists(lists);

            while (!int.TryParse(Console.ReadLine(), out answer) || answer < 1 || answer > lists.Count)
            {
                Console.WriteLine("Некорректный ввод. Попробуйте ещё раз\n");
            }

            Console.WriteLine("Вы уверены, что хотите удалить \"{0}\"? (y/n)\n", lists[answer - 1].Title);

            if (!Console.ReadLine().Equals("y"))
            {
                Console.WriteLine("Удаление отменено");
                return;
            }

            lists.RemoveAt(answer - 1);

            Console.WriteLine("Удалено!");

            Console.WriteLine("Нажмите <Enter> для возврата в главное меню");
            Console.ReadLine();
            Console.Clear();
        }

        private static void PrintSubscriptionOfUser()
        {
            var users = core.GetUsers();
            int answer;

            Console.WriteLine("Выберите пользователя для показа его подписок\n");
            PrintUsers(users);

            while (!int.TryParse(Console.ReadLine(), out answer) || answer < 1 || answer > users.Count)
            {
                Console.WriteLine("Некорректный ввод. Попробуйте ещё раз\n");
            }
            var selectedUser = users[answer - 1];
            Console.WriteLine("Список подписок пользователя {0} {1} {2} ({3})", selectedUser.FirstName, selectedUser.LastName, selectedUser.MiddleName, selectedUser.Login);
            var selectedLists = core.GetDistributeListsOfUser(selectedUser);
            for (int i = 0; i < selectedLists.Count; i++)
            {
                Console.WriteLine("\t{0}. {1}", i + 1, selectedLists[i].Title);
            }
            if (selectedLists.Count == 0)
            {
                Console.WriteLine("Список пуст");
            }

            Console.WriteLine("Нажмите <Enter> для возврата в главное меню");
            Console.ReadLine();
            Console.Clear();
        }

        private static void PrintCandidatesForDeletion()
        {
            var candidates = core.GetCandidatesForDeletion();

            Console.WriteLine("Список рассылок (кандидатов на удаление)");
            for (int i = 0; i < candidates.Count; i++)
            {
                Console.WriteLine("\t{0}. {1}", i + 1, candidates[i].Title);
            }

            Console.WriteLine("Нажмите <Enter> для возврата в главное меню");
            Console.ReadLine();
            Console.Clear();
        }

        private static void AddUserToList()
        {
            var users = core.GetUsers();
            var lists = core.GetDistributeLists();
            int answer;

            Console.WriteLine("Выберите пользователя для добавления в список рассылки\n");
            PrintUsers(users);

            while (!int.TryParse(Console.ReadLine(), out answer) || answer < 1 || answer > users.Count)
            {
                Console.WriteLine("Некорректный ввод. Попробуйте ещё раз\n");
            }
            var selectedUser = users[answer - 1];

            Console.WriteLine("Выберите рассылку, в которую необходимо добавить пользователя\n");

            var distributeListsWithoutUser = core.GetDistrListWithoutUsers(lists, selectedUser);

            PrintDistributeLists(distributeListsWithoutUser);

            if (distributeListsWithoutUser.Count != 0)
            {
                while (!int.TryParse(Console.ReadLine(), out answer) || answer < 1 || answer > lists.Count)
                {
                    Console.WriteLine("Некорректный ввод. Попробуйте ещё раз\n");
                }
                var selectedList = distributeListsWithoutUser.ToList<DistributeList>()[answer - 1];
                core.AddUserToDistributeList(selectedUser, selectedList);
                lists = core.GetDistributeListsOfUser(selectedUser);
                Console.WriteLine("Пользователь добавлен");
            }

            Console.WriteLine("Нажмите <Enter> для возврата в главное меню");
            Console.ReadLine();
            Console.Clear();
        }
    }
}
