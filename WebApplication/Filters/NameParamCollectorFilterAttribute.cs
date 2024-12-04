using System;
using System.Collections.Generic;
using Common.Supports;
using DotNetCommon.Data;
using DotNetCommon.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Spring.Expressions;
using YW.ModelManagement.Common;

namespace WebApplication.Filters;

public class NameParamCollectorFilterAttribute : ActionFilterAttribute
{
    public new int Order { get; set; } = -50; // 比 CacheFileter 先执行
    public string ParamPath { get; set; }

    private NameParamCollectInfo _collectInfo;

    // static ILogger<NameParamCollectorFilterAttribute> _logger = LoggerFactory.CreateLogger<NameParamCollectorFilterAttribute>();


    /// <summary>
    /// 当执行完毕时，处理参数
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (_collectInfo == null) // 符合要求的才处理
        {
            return;
        }

        ParseReturnValue(context);
    }

    private void ParseReturnValue(ActionExecutedContext context)
    {
        if (context.Result is not ObjectResult objectResult) return;

        var result = objectResult.Value;
        if (result == null) return;


        var resultGenericType = result.GetType().GetGenericTypeDefinition();
        var isResultWrapper = resultGenericType == typeof(Result<>) || resultGenericType == typeof(Result);
        var isResWrapper = resultGenericType == typeof(Res<>) || resultGenericType == typeof(Res);

        if (!isResultWrapper && !isResWrapper)
        {
            return; // 未知响应不处理
        }

        var genericTypeArgument = result.GetType().GenericTypeArguments[0]; // 响应体payload的泛型
        var typeDefinition = genericTypeArgument.GetGenericTypeDefinition(); // 泛型定义
        var payloadIsList = typeDefinition == typeof(List<>) || typeDefinition == typeof(IList<>);
        var payloadIsPage = typeDefinition == typeof(Page<>);
        var payloadIsPageExt = typeDefinition == typeof(PageExt<>);

        var countExpression = "";
        var payloadExpPrefix = isResultWrapper ? "Data" : "Payload";


        if (payloadIsList)
        {
            countExpression = $"{payloadExpPrefix}.Count";
        }
        else if (payloadIsPage)
        {
            countExpression = $"{payloadExpPrefix}.TotalCount";
        }
        else if (payloadIsPageExt)
        {
            countExpression = $"{payloadExpPrefix}.Page.TotalCount";
        }
        
        try
        {
            var countValue = ExpressionEvaluator.GetValue(result, countExpression)?.To<int>();
            _collectInfo.ResultCount = countValue;
        }
        catch (Exception e)
        {
            var logger = (ILogger<NameParamCollectorFilterAttribute>)context.HttpContext.RequestServices.GetService(typeof(ILogger<NameParamCollectorFilterAttribute>));
            logger?.LogWarning($"ExpressionEvaluator error as {e.Message} when {countExpression} for {typeDefinition.Name}");
        }
        // 发送mq
    }

    /// <summary>
    /// 当执行完毕时，处理参数
    /// </summary>
    /// <param name="context"></param>
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var paramMap = context.ActionArguments;
        var searchValue = SpelExpressionParserUtils.GenerateKeyByEl(paramMap, ParamPath);
        if (searchValue.IsNotNullOrEmptyOrWhiteSpace())
        {
            _collectInfo = new NameParamCollectInfo
            {
                Name = searchValue,
                UriPath = context.HttpContext.Request.Path.ToString()
            };
        }
    }
}

public class NameParamCollectInfo
{
    public string UriPath { get; set; }

    /// <summary>
    /// 用户录入的搜索参数
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 搜索结果的个数
    /// </summary>
    public int? ResultCount { get; set; }

    public object Result { get; set; }
    public IDictionary<string, object> Params { get; set; }
}