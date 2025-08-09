using System;
using ControleReservas.MVC.Models;

namespace ControleReservas.MVC.Services;

public class UsuarioApiService : IUsuarioApiService
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public UsuarioApiService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _baseUrl = config.GetValue<string>("ApiSettings:BaseUrl", string.Empty).TrimEnd('/');
    }


    public async Task<IEnumerable<UsuarioViewModel>> ObterUsuariosAsync()
    {
        var response = await _http.GetFromJsonAsync<IEnumerable<UsuarioViewModel>>($"{_baseUrl}/usuarios");

        return response ?? Enumerable.Empty<UsuarioViewModel>();
    }

    public async Task<UsuarioViewModel?> ObterPorIdAsync(Guid id)
    {
        var response = await _http.GetFromJsonAsync<UsuarioViewModel>($"{_baseUrl}/usuarios/{id}");

        return response;
    }

    public async Task CriarAsync(UsuarioViewModel vm)
    {
        var response = await _http.PutAsJsonAsync($"{_baseUrl}/usuarios", vm);

        response.EnsureSuccessStatusCode();
    }

    public async Task AtualizarAsync(UsuarioViewModel vm)
    {
        var response = await _http.PutAsJsonAsync($"{_baseUrl}/usuarios/{vm.Id}", vm);

        response.EnsureSuccessStatusCode();
    }

    public async Task RemoverAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"{_baseUrl}/usuarios/{id}");
        response.EnsureSuccessStatusCode();
    }    

}
