using System;
using System.Collections.Generic;

namespace TestProject.Cache;
#nullable enable
public readonly struct Optional<TValue> : IEquatable<Optional<TValue>>
{
    /// <summary>
    /// 空值表达：
    /// 1、Value结果值，可能为null。null代表可能是有效的，即保存的结果就是需要null，可能是无效的，即没有这种结果值，null用来表达无效、不存在等。
    /// 2、HasValue表示Value是不是个有效值，即使是null
    /// 方便的方法属性：
    /// 1、ValueIsNull->Value是否是空的。只关注拿到的Value，不关注究竟是不是有效时可以用
    ///
    /// 特例：
    /// 对于原生类型，可能存在这种情况，即 ValueIsNull 是false，但是 HasValue 也是false。因为构建时，Value必须赋值。所以对于非引用类型，需要特意留意 HasValue 来判定结果是否有效。
    /// </summary>
    public Optional()
    {
        Value = default;
        HasValue = true;
    }

    public Optional(TValue? value, bool hasValue = true)
    {
        HasValue = hasValue;
        Value = HasValue ? value : default;
    }

    public TValue? Value { get; }

    /// <summary>
    /// 是否有值。注意，HasValue==true时也可能 Value 是null。因为有特意指定空值的时候，这里选择了对此动作进行区分。
    /// </summary>
    public bool HasValue { get; }

    public bool ValueIsNull => Value == null;

    public static Optional<TValue?> FromValue(TValue? value, bool nullAsValue = true)
    {
        var hasValue = nullAsValue || value != null;
        return new Optional<TValue?>(value, hasValue);
    }

    public static Optional<TValue?> FromHasValue(TValue? value, bool hasValue)
    {
        return new Optional<TValue?>(value, hasValue);
    }


    public TValue? GetValueOrDefault()
    {
        return HasValue ? Value : default;
    }

    public TValue? GetValueOrDefault(TValue? defaultValue)
    {
        return HasValue ? Value : defaultValue;
    }

    public override string? ToString()
    {
        return HasValue ? Value?.ToString() : string.Empty;
    }

    public static implicit operator Optional<TValue?>(TValue? value)
    {
        return FromValue(value);
    }


    public static implicit operator TValue?(Optional<TValue?> maybe)
    {
        return maybe.Value;
    }

    public static bool operator ==(Optional<TValue?> left, Optional<TValue?> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Optional<TValue?> left, Optional<TValue?> right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        // return (obj is Optional<TValue> value && Equals(value))
        //        || (obj is Optional<TValue?> value2 && ((IEquatable<Optional<TValue?>>) this).Equals(value2));
        return obj is Optional<TValue> value && Equals(value);
    }

    public bool Equals(Optional<TValue> other)
    {
        return EqualityComparer<TValue>.Default.Equals(Value, other.Value);
    }

    public override int GetHashCode()
    {
        return ValueIsNull ? 0 : EqualityComparer<TValue>.Default.GetHashCode(Value!);
    }
}