using System;
using ControleReservas.MVC.Models;

namespace ControleReservas.MVC.Services;

public class SalaApiService : ISalaApiService
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public SalaApiService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _baseUrl = config.GetValue<string>("ApiSettings:BaseUrl", string.Empty).TrimEnd('/');
    }


    public async Task<IEnumerable<SalaViewModel>> ObterSalasAsync()
    {
        var response = await _http.GetFromJsonAsync<IEnumerable<SalaViewModel>>($"{_baseUrl}/salas");

        return response ?? Enumerable.Empty<SalaViewModel>();
    }

    public async Task<SalaViewModel?> ObterPorIdAsync(Guid id)
    {
        var response = await _http.GetFromJsonAsync<SalaViewModel>($"{_baseUrl}/sala/{id}");

        return response;
    }

    public async Task CriarAsync(SalaViewModel vm)
    {
        var response = await _http.PutAsJsonAsync($"{_baseUrl}/salas", vm);

        response.EnsureSuccessStatusCode();
    }

    public async Task AtualizarAsync(SalaViewModel vm)
    {
        var response = await _http.PutAsJsonAsync($"{_baseUrl}/salas/{vm.Id}", vm);

        response.EnsureSuccessStatusCode();
    }

    public async Task RemoverAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"{_baseUrl}/salas/{id}");
        response.EnsureSuccessStatusCode();
    }
}
