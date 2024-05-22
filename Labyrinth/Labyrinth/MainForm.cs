using System;
using System.Drawing;
using System.Windows.Forms;

namespace Labyrinth
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            // Создание нового экземпляра таймера
            Timer timer = new Timer();
            // Установка интервала таймера на 100 миллисекунд
            timer.Interval = 100;
            // Привязка обработчика события Tick к методу Timer_Tick
            timer.Tick += Timer_Tick;
            // Запуск таймера, который будет генерировать событие Tick с указанным интервалом
            timer.Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            int step = 5; // шаг движения

            switch (e.KeyCode)
            {
                case Keys.Left:
                    MoveMainLabel(-step, 0); // влево на шаг
                    break;
                case Keys.Right:
                    MoveMainLabel(step, 0); // вправо на шаг
                    break;
                case Keys.Up:
                    MoveMainLabel(0, -step); // вверх на шаг
                    break;
                case Keys.Down:
                    MoveMainLabel(0, step); // вниз на шаг
                    break;
            }
        }
        private int lives = 3;//переменная жизней
        int score = 0;//переменная счёта
        private void CheckCollisionAndHidePictureBoxes()
        {
            // Перебираем все контролы на форме
            foreach (Control control in Controls)
            {
                // Проверяем, что контрол не является главным PictureBox
                // и является PictureBox
                if (control != MainPictureBox && control is PictureBox)
                {
                    PictureBox targetPictureBox = (PictureBox)control;

                    // Проверяем пересечение границ главного PictureBox с финишным PictureBox
                    // и что контрол - финишный PictureBox
                    if (MainPictureBox.Bounds.IntersectsWith(FinishPictureBox.Bounds) && targetPictureBox == FinishPictureBox)
                    {
                        // Проверяем видимость финишного PictureBox перед выводом сообщения
                        if (FinishPictureBox.Visible == true)
                        {
                            MessageBox.Show($"Вы достигли финиша!\nСобрали {score} мешочков из 7.\nИгра окончена.");
                            Application.Exit(); // Завершаем приложение
                        }
                    }

                    // Обрабатываем столкновения главного PictureBox с другими PictureBox
                    if (MainPictureBox.Bounds.IntersectsWith(targetPictureBox.Bounds) && targetPictureBox.Visible)
                    {
                        // Реакция на столкновение с Enemy1
                        if (targetPictureBox == Enemy1)
                        {
                            // Перемещаем Enemy1 на начальную позицию
                            Enemy1.Location = initialPosition1;
                            lives--; // Уменьшаем количество жизней
                            this.Text = $"Лабиринт, счёт: {score}, жизни: {lives}";

                            // Проверка на окончание жизней
                            if (lives <= 0)
                            {
                                MessageBox.Show("Вы проиграли! Игра окончена.");
                                Application.Exit(); // Завершаем приложение
                            }
                        }
                        // Реакция на столкновение с Enemy2
                        else if (targetPictureBox == Enemy2)
                        {
                            // Перемещаем Enemy2 на начальную позицию
                            Enemy2.Location = initialPosition2;
                            lives--;
                            this.Text = $"Лабиринт, счёт: {score}, жизни: {lives}";

                            if (lives <= 0)
                            {
                                MessageBox.Show("Вы проиграли! Игра окончена.");
                                Application.Exit(); // Завершаем приложение
                            }
                        }
                        // Реакция на столкновение с Enemy3
                        else if (targetPictureBox == Enemy3)
                        {
                            // Перемещаем Enemy3 на начальную позицию
                            Enemy3.Location = initialPosition3;
                            lives--;
                            this.Text = $"Лабиринт, счёт: {score}, жизни: {lives}";

                            if (lives <= 0)
                            {
                                MessageBox.Show("Вы проиграли! Игра окончена.");
                                Application.Exit(); // Завершаем приложение
                            }
                        }
                        // Действия при столкновении с другими PictureBox
                        else
                        {
                            targetPictureBox.Visible = false;
                            if (targetPictureBox != FinishPictureBox)
                            {
                                score++; // Увеличиваем счёт
                                if (score == 4)//отображение финиша в зависимости от счёта
                                {
                                    FinishPictureBox.Visible = true;
                                }
                                this.Text = $"Лабиринт, счёт: {score}, жизни: {lives}";
                            }
                        }
                    }
                }
            }
        }
        // Функция для проверки столкновения двух контролов (Control) по их границам
        bool CheckCollision(Control control1, Control control2)
        {
            return control1.Bounds.IntersectsWith(control2.Bounds);
        }

        // Метод для перемещения главного PictureBox с учетом возможных столкновений с метками (Label)
        private void MoveMainLabel(int deltaX, int deltaY)
        {
            // Вычисляем целевые координаты после перемещения
            int targetX = MainPictureBox.Left + deltaX;
            int targetY = MainPictureBox.Top + deltaY;

            bool collisionDetected = false; // Флаг обнаружения столкновения
            Point originalPos = new Point(MainPictureBox.Left, MainPictureBox.Top); // Сохраняем начальные координаты

            // Проверяем столкновения с метками
            foreach (Control obstacle in Controls)
            {
                if (obstacle != MainPictureBox && obstacle is Label)
                {
                    if (CheckCollision(MainPictureBox, obstacle)) // Проверяем столкновение
                    {
                        collisionDetected = true; // Устанавливаем флаг столкновения
                        break; // Прерываем цикл при обнаружении столкновения
                    }
                }
            }

            // Обработка перемещения в зависимости от обнаруженного столкновения
            if (collisionDetected)
            {
                MainPictureBox.Left -= deltaX * 2; // Возвращаемся к исходной позиции при столкновении
                MainPictureBox.Top -= deltaY * 2;
            }
            else
            {
                MainPictureBox.Left = targetX; // Перемещаем объект на целевые координаты
                MainPictureBox.Top = targetY;
            }
        }

        private void MainPictureBox_Move(object sender, System.EventArgs e)
        {
            CheckCollisionAndHidePictureBoxes();
        }
        private Point initialPosition1 = new Point(165, 216); // Начальная позиция Enemy1
        private Point targetPosition1 = new Point(350, 216); // Целевая позиция Enemy1
        private Point initialPosition2 = new Point(490, 318); // Начальная позиция Enemy2
        private Point targetPosition2 = new Point(490, 214); // Целевая позиция Enemy2
        private Point initialPosition3 = new Point(370, 16);  // Начальная позиция Enemy3
        private Point targetPosition3 = new Point(210, 16); // Целевая позиция Enemy3

        // Обработчик события таймера, отвечающий за перемещение врагов на каждом тике
        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveEnemy(Enemy1, ref initialPosition1, ref targetPosition1); // Передвижение Enemy1
            MoveEnemy(Enemy2, ref initialPosition2, ref targetPosition2); // Передвижение Enemy2
            MoveEnemy(Enemy3, ref initialPosition3, ref targetPosition3); // Передвижение Enemy3
        }

        // Метод для перемещения врага к целевой точке с учетом порогового значения и возврата к начальной точке
        private void MoveEnemy(Control enemy, ref Point initialPos, ref Point targetPos)
        {
            int speed = 4; // Скорость перемещения
            int threshold = 2; // Пороговое значение для определения достижения цели

            int deltaX = targetPos.X - enemy.Left; // Разница по X между текущей и целевой позициями
            int deltaY = targetPos.Y - enemy.Top; // Разница по Y между текущей и целевой позициями

            // Проверяем, достиг ли враг целевой позиции с учетом порогового значения
            if (Math.Abs(deltaX) <= threshold && Math.Abs(deltaY) <= threshold)
            {
                // Если достигнуто пороговое значение, меняем целевую и начальную позиции местами
                Point temp = targetPos;
                targetPos = initialPos;
                initialPos = temp;
            }
            else
            {
                // Перемещаем врага в направлении целевой точки с заранее заданной скоростью
                if (enemy.Left < targetPos.X)
                {
                    enemy.Left += speed;
                }
                else if (enemy.Left > targetPos.X)
                {
                    enemy.Left -= speed;
                }

                if (enemy.Top < targetPos.Y)
                {
                    enemy.Top += speed;
                }
                else if (enemy.Top > targetPos.Y)
                {
                    enemy.Top -= speed;
                }
            }
        }

    }
}
