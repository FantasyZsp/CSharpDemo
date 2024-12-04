using System;
using System.Threading.Tasks;
using TestProject.BaseApi.Models;
using Xunit;

namespace TestProject.Cache;
#nullable enable
public class LoadingCacheTest
{
    [Fact]
    public async Task TryGetStringTest()
    {
        var loadingCache = new LoadingCache<string, string>(Task.FromResult!, TimeSpan.FromMilliseconds(1000));
        var getOrLoadAsync = await loadingCache.GetOrLoadAsync("string");
        Assert.True(getOrLoadAsync == "string");

        var missTryGetAsync = await loadingCache.TryGetAsync("miss");
        Assert.True(missTryGetAsync.Value == null);
        Assert.True(missTryGetAsync.ValueIsNull);
        Assert.False(missTryGetAsync.HasValue);

        var content2 = await loadingCache.TryGetAsync("xxxx");
        Assert.True(content2 == null);
        await Task.CompletedTask;
    }

    [Fact]
    public async Task TryGetIntTest()
    {
        var loadingCache = new LoadingCache<string, int>(key => Task.FromResult(key.Length), TimeSpan.FromMilliseconds(1000));
        var getOrLoadAsync = await loadingCache.GetOrLoadAsync("string");
        Assert.True(getOrLoadAsync == "string".Length);


        var getOrLoadAsync2 = await loadingCache.GetOrLoadAsync("string");
        Assert.True(getOrLoadAsync2 == "string".Length);


        var missTryGetAsync1 = await loadingCache.TryGetAsync("string");
        Assert.True(missTryGetAsync1 == "string".Length);
        Assert.True(missTryGetAsync1.Value == "string".Length);
        Assert.True(!missTryGetAsync1.ValueIsNull);
        Assert.True(missTryGetAsync1.HasValue);

        var missTryGetAsync2 = await loadingCache.TryGetAsync("miss");
        Assert.True(missTryGetAsync2.Value == default);
        Assert.True(!missTryGetAsync2.ValueIsNull);
        Assert.False(missTryGetAsync2.HasValue);
    }

    [Fact]
    public async Task? TryGetIntNullableTest()
    {
        var loadingCache = new LoadingCache<string, int?>(key => Task.FromResult(key?.Length), TimeSpan.FromMilliseconds(1000));
        var getOrLoadAsync = await loadingCache.GetOrLoadAsync("string");
        Assert.True(getOrLoadAsync == "string".Length);


        var getOrLoadAsync2 = await loadingCache.GetOrLoadAsync("string");
        Assert.True(getOrLoadAsync2 == "string".Length);


        var hits = await loadingCache.TryGetAsync("string");
        Assert.True(hits == "string".Length);
        Assert.True(hits.Value == "string".Length);
        Assert.True(!hits.ValueIsNull);
        Assert.True(hits.HasValue);

        var missTryGetAsync2 = await loadingCache.TryGetAsync("miss");
        Assert.True(missTryGetAsync2.Value == default);
        Assert.True(missTryGetAsync2.ValueIsNull);
        Assert.False(missTryGetAsync2.HasValue);
    }

    // #nullable disable
    [Fact]
    public async Task? TryGetKidNullableTest()
    {
        var kidStr = Kid.Of("string");
        var loadingCache = new LoadingCache<string, Kid?>(key => Task.FromResult(Kid.Of(key))!, TimeSpan.FromMilliseconds(1000));
        var getOrLoadAsync = await loadingCache.GetOrLoadAsync("string");
        Assert.True(getOrLoadAsync != null && getOrLoadAsync.Equals(kidStr));
        

        var hits = await loadingCache.TryGetAsync("string");
        Assert.True(hits == kidStr);
        Assert.True(Equals(hits.Value, kidStr));
        Assert.True(!hits.ValueIsNull);
        Assert.True(hits.HasValue);

        var missTryGetAsync2 = await loadingCache.TryGetAsync("miss");
        Assert.True(missTryGetAsync2 == null);
        Assert.True(missTryGetAsync2.Value == null);
        Assert.True(missTryGetAsync2.Value == default);
        Assert.True(missTryGetAsync2.ValueIsNull);
        Assert.False(missTryGetAsync2.HasValue);
    }
}