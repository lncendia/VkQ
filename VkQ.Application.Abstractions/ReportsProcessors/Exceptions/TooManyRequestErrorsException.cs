﻿namespace VkQ.Application.Abstractions.ReportsProcessors.Exceptions;

public class TooManyRequestErrorsException : Exception
{
    public int Code { get; }
    public override string Message { get; }

    public TooManyRequestErrorsException(int code)
    {
        Code = code;
        Message = code switch
        {
            5 => "Авторизация пользователя не удалась",
            6 => "Слишком много запросов в секунду",
            7 => "Нет прав для выполнения этого действия",
            8 => "Неверный запрос",
            9 => "Слишком много однотипных действий",
            10 => "Произошла внутренняя ошибка сервера",
            14 => "Требуется ввод кода с картинки (Captcha)",
            17 => "Требуется валидация пользователя",
            18 => "Страница удалена или заблокирована",
            29 => "Достигнут количественный лимит на вызов метода ",
            30 => "Профиль является приватным",
            _ => "Неизвестная ошибка"
        };
    }
}