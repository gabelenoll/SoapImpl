using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DEF = Voith.WF.OrderReleaseVP.Business.Defintions;
using FORM = Voith.WF.OrderReleaseVP.Business.Model.OrvpForm;
using MODEL = Voith.WF.OrderReleaseVP.Business.Model;
using RWFH = Voith.WF.OrderReleaseVP.Business.Repositories.Rest.WFHelper;
using SWFH = Voith.WF.OrderReleaseVP.Business.Repositories.Soap.WFHelper;
using WEBR = Voith.WF.OrderReleaseVP.Business.Repositories.Web;

namespace Voith.WF.OrderReleaseVP.Business.Controllers;

public interface IWFHelperController
{
    Task<IReadOnlyDictionary<string, string>> GetMachineCodewordsAsync();
    Task<FORM.LookupMachineData> GetMachineByCodewordAsync(string codeword);
    Task<MODEL.TreeItemUiData> GetSupplyScopeAsync(string language);
    Task<IEnumerable<string>> GetPartnerCompaniesAsync();
    Task<IEnumerable<MODEL.TreeItemUiData>> GetDistributionListAsync(string partnerCompany, string language);
    Task<IEnumerable<MODEL.StoredFormInfo>?> GetStoredFormsAsync(string owner);
    Task<FORM.OrvpForm> RetrieveFormAsync(string owner, string id);
    Task StoreFormAsync(FORM.OrvpForm form, string initiatorShortName, string initialLanguage);
    Task DeleteFormAsync(string id);
    Task InitiateWorkflowAsync(FORM.OrvpForm form, string initiatorShortName);
    Task<string> GetRevisionUrlAsync(string id);
}

internal sealed class WFHelperController : IWFHelperController
{
    private RWFH.WFHelperRestService _restService;
    private SWFH.WFHelperSoapService _soapService;
    private SWFH.WFHelperSharePointSoapService _sharePointSoapService;
    private WEBR.RevisionUrlsService _revisionUrlsService;

    private Task<IReadOnlyDictionary<string, string>>? _getMachineCodewordsCache;
    private WFHelperControllerCacheHandler<object, IReadOnlyDictionary<string, string>> _getMachineCodewordsCacheHandler;

    private Dictionary<string, Task<MODEL.TreeItemUiData>> _getSupplyScopeCache;
    private WFHelperControllerCacheHandler<string, MODEL.TreeItemUiData> _getSupplyScopeCacheHandler;

    private Task<Queue<string>>? _getPartnerCompaniesCache;
    private WFHelperControllerCacheHandler<object, Queue<string>> _getPartnerCompaniesCacheHandler;

    public WFHelperController(HttpClient httpClient, DEF.AppSettings settings)
    {
        _restService = new RWFH.WFHelperRestService(httpClient, settings);
        _soapService = new SWFH.WFHelperSoapService(httpClient, settings);
        _sharePointSoapService = new SWFH.WFHelperSharePointSoapService(httpClient, settings);
        _revisionUrlsService = new WEBR.RevisionUrlsService(httpClient, settings);

        _getMachineCodewordsCache = null;
        _getMachineCodewordsCacheHandler = new WFHelperControllerCacheHandler<object, IReadOnlyDictionary<string, string>>();
        
        _getSupplyScopeCache = new Dictionary<string, Task<MODEL.TreeItemUiData>>();
        _getSupplyScopeCacheHandler = new WFHelperControllerCacheHandler<string, MODEL.TreeItemUiData>();
        
        _getPartnerCompaniesCache = null;
        _getPartnerCompaniesCacheHandler = new WFHelperControllerCacheHandler<object, Queue<string>>();
    }

    public Task<IReadOnlyDictionary<string, string>> GetMachineCodewordsAsync()
    {
        async Task<IReadOnlyDictionary<string, string>> Acquire()
        {
            IReadOnlyDictionary<string, string>? ret = await _restService.GetMachineCodewordsAsync();
            if(ret == null)
                throw WFHelperControllerException.UnexpectedNullValueFromService;
            return ret;
        }

        return _getMachineCodewordsCacheHandler.Get(
            null!,
            (object _, out Task<IReadOnlyDictionary<string, string>> result) => {
                if(_getMachineCodewordsCache != null)
                {
                    result = _getMachineCodewordsCache;
                    return true;
                }
                else
                {
                    result = null!;
                    return false;
                }
            },
            (_, result) => { _getMachineCodewordsCache = result; },
            Acquire,
            (_, result) => { _getMachineCodewordsCache = result; },
            1088514
        );
    }

    public async Task<FORM.LookupMachineData> GetMachineByCodewordAsync(string codeword)
    {
        MODEL.WFMachineData? ret = await _restService.GetMachineByCodewordAsync(codeword);
        if(ret == null)
            throw WFHelperControllerException.UnexpectedNullValueFromService;
        return new FORM.LookupMachineData(ret);
    }

    public Task<MODEL.TreeItemUiData> GetSupplyScopeAsync(string language)
    {
        async Task<MODEL.TreeItemUiData> Acquire()
        {
            const string listName = "{E3A1D784-46E6-4290-995E-DBE09D0AE904}";

            SWFH.GetListItemsResponseMessage response = await _sharePointSoapService.GetListItemsAsync(listName);
            string? error = response.Error;
            if(error != null)
                throw new WFHelperControllerException(error);

            MODEL.TreeItemUiData? ret = await _restService.GetSupplyScopeAsync(language, new HashSet<string>(response.Result));
            if(ret == null)
                throw WFHelperControllerException.UnexpectedNullValueFromService;
            return ret;
        }

        return _getSupplyScopeCacheHandler.Get(
            language,
            (string language, out Task<MODEL.TreeItemUiData> result) => { return _getSupplyScopeCache.TryGetValue(language, out result!); },
            (language, result) => { _getSupplyScopeCache.Add(language, result); },
            Acquire,
            (language, result) => { _getSupplyScopeCache[language] = result; },
            849761
        );
    }

    public async Task<IEnumerable<string>> GetPartnerCompaniesAsync()
    {
        async Task<Queue<string>> Acquire()
        {
            SWFH.GetPartnerCompaniesResponseMessage response = await _soapService.GetPartnerCompaniesAsync();
            string? error = response.Error;
            if(error != null)
                throw new WFHelperControllerException(error);
            return new Queue<string>(response.Result);
        }

        return await _getPartnerCompaniesCacheHandler.Get(
            null!,
            (object _, out Task<Queue<string>> result) => {
                if(_getPartnerCompaniesCache != null)
                {
                    result = _getPartnerCompaniesCache;
                    return true;                    
                }
                else
                {
                    result = null!;
                    return false;
                }
            },
            (_, result) => { _getPartnerCompaniesCache = result; },
            Acquire,
            (_, result) => { _getPartnerCompaniesCache = result; },
            1207803
        );
    }

    public async Task<IEnumerable<MODEL.TreeItemUiData>> GetDistributionListAsync(string partnerCompany, string language)
    {
        const string CENTRAL_DISTRIBUTION_LIST = "VP";

        var dict = new Dictionary<string, Tuple<MODEL.TreeItemUiData, Queue<MODEL.TreeItemUiData>>>();

        IEnumerable<MODEL.TreeItemUiData> Return()
        {
            foreach(Tuple<MODEL.TreeItemUiData, Queue<MODEL.TreeItemUiData>> data in dict.Values)
            {
                data.Item1.Children = data.Item2.ToArray();
                yield return data.Item1;
            }
        }

        async Task GetListAsync(string by) {
            SWFH.GetDistributionListResponseMessage response = await _soapService.GetDistributionListAsync(by, language);
            string? error = response.Error;
            if(error != null)
                throw new WFHelperControllerException(error);

            foreach(MODEL.DistributionListItem item in response.Result)
            {
                if(item.Category == null || item.Name == null)continue;
                Tuple<MODEL.TreeItemUiData, Queue<MODEL.TreeItemUiData>>? data;

                if(! dict.TryGetValue(item.Category, out data))
                    dict.Add(
                        item.Category,
                        data = Tuple.Create(
                            new MODEL.TreeItemUiData{Text = item.Category, IsSelected = false},
                            new Queue<MODEL.TreeItemUiData>()
                        )
                    );
                data.Item2.Enqueue(new MODEL.TreeItemUiData{Text = item.Name, IsSelected = false});
            }
        }

        await GetListAsync(CENTRAL_DISTRIBUTION_LIST);
        if(partnerCompany != CENTRAL_DISTRIBUTION_LIST)
            await GetListAsync(partnerCompany);

        return Return();
    }

    public Task<IEnumerable<MODEL.StoredFormInfo>?> GetStoredFormsAsync(string owner)
    {
        return _restService.GetStoredFormsAsync(owner);
    }

    public Task<FORM.OrvpForm> RetrieveFormAsync(string owner, string id)
    {
        return _restService.RetrieveFormAsync(owner, id);
    }

    public Task StoreFormAsync(FORM.OrvpForm form, string initiatorShortName, string initialLanguage)
    {
        return _restService.StoreFormAsync(form, initiatorShortName, initialLanguage);
    }

    public Task DeleteFormAsync(string id)
    {
        return _restService.DeleteFormAsync(id);
    }

    public Task InitiateWorkflowAsync(FORM.OrvpForm form, string initiatorShortName)
    {
        return _restService.InitiateWorkflowAsync(form, initiatorShortName);
    }

    public Task<string> GetRevisionUrlAsync(string id)
    {
        return _revisionUrlsService.GetRevisionUrlAsync(id);
    }
}