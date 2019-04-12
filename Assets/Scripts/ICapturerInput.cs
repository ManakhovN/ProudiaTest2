using System;

internal interface ICapturerInput
{
    event Action OnChoosePathClick;
    event Action OnSaveScreenshot;
}