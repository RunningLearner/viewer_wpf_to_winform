// Bindable.cs
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace dicom_viewer_winform.Entities;  // 파일 범위 네임스페이스

/// <summary>
/// INotifyPropertyChanged 구현 클래스: 
/// 프로퍼티 변경 시 SetProperty 를 통해 자동으로 알림을 발생시킵니다.
/// </summary>
public abstract class Bindable : INotifyPropertyChanged
{
    #region INotifyPropertyChanged
    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion

    #region Protected Methods
    /// <summary>
    /// 필드 값을 비교·갱신하고, 변경되었으면 PropertyChanged 이벤트를 발생시킵니다.
    /// </summary>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        return true;
    }
    #endregion
}
