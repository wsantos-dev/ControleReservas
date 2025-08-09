using System;
using ControleReservas.MVC.Models;

namespace ControleReservas.MVC.Services;

public class ReservaApiService : IReservaApiService
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;


    public ReservaApiService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _baseUrl = config.GetValue<string>("ApiSettings:BaseUrl")!.TrimEnd('/');
    }


    public async Task<IEnumerable<ReservaViewModel>> ObterReservasAsync()
    {
        var response = await _http.GetFromJsonAsync<IEnumerable<ReservaViewModel>>($"{_baseUrl}/reservas");
        return response ?? Enumerable.Empty<ReservaViewModel>(); 
    }

    public async Task<ReservaViewModel?> ObterPorIdAsync(Guid id)
    {
        var response = await _http.GetFromJsonAsync<ReservaViewModel>($"{_baseUrl}/reservas/{id}");

        return response;
    }

    public async Task CriarAsync(ReservaViewModel vm)
    {
        var payload = new
        {
            salaId = vm.SalaId,
            usuarioId = vm.UsuarioId,
            DataHoraInicio = vm.DataHoraInicio,
            DataHoraFim = vm.DataHoraFim,
            DataCancelamento = vm.DataCancelamento
        };

        var response = await _http.PostAsJsonAsync($"{_baseUrl}/reservas", payload);
        response.EnsureSuccessStatusCode();
    }

    public async Task CancelarAsync(Guid id)
    {
        var response = await _http.PutAsync($"{_baseUrl}/reservas/{id}/cancelar", null);
        response.EnsureSuccessStatusCode();

    }
}
