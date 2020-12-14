using ExpenseTracker.API.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.API.Dtos.Wallets;
using ExpenseTracker.API.Repositories.Interfaces;
using ExpenseTracker.Domain.Auth;
using ExpenseTracker.Domain.Wallets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserInfoRepository _personRepository;

        public WalletsController(IWalletRepository repository, IMapper mapper,
            UserManager<User> userManager, IAuthorizationService authorizationService,
            IUserInfoRepository personRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _authorizationService = authorizationService;
            _personRepository = personRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWallet(int id)
        {
            var wallet = await _repository.Get(id);
            if(wallet == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, wallet, "Permission");
            if (!AR.Succeeded)
            {
                return Forbid();
            }

            var walletDto = _mapper.Map<WalletDto>(wallet);
            return Ok(walletDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWallets()
        {
            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, "Permission");
            if (!AR.Succeeded)
            {
                return Unauthorized();
            }

            var wallets = await _repository.GetAll();

            List<WalletDto> walletsDto = new List<WalletDto>();
            wallets.ToList().ForEach(wallet => walletsDto.Add(_mapper.Map<WalletDto>(wallet)));
            return Ok(walletsDto);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserWallets(int userId)
        {
            var wallets = _repository.Where(p => p.OwnerId == userId);

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, wallets, "Permission");
            if (!AR.Succeeded)
            {
                return Forbid();
            }

            List<WalletDto> walletsDto = new List<WalletDto>();
            wallets.ToList().ForEach(wallet => walletsDto.Add(_mapper.Map<WalletDto>(wallet)));
            return Ok(walletsDto);
        }

        [HttpGet("list")]
        public IActionResult GetWalletsForList()
        {
            var userId = int.Parse(HttpContext.GetUserIdFromToken());
            var wallets = _repository.Where(p => p.OwnerId == userId).ToList();

            List<WalletForListDto> walletsForListDtos = new List<WalletForListDto>();

            wallets.ForEach(p => walletsForListDtos.Add(_mapper.Map<WalletForListDto>(p)));

            return Ok(walletsForListDtos);
        }

        [HttpGet("currentUser")]
        public IActionResult GetCurrentUserWallets()
        {
            var userId = int.Parse(HttpContext.GetUserIdFromToken());
            var wallets = _repository.Where(p => p.OwnerId == userId);

            List<WalletDto> walletsDto = new List<WalletDto>();
            wallets.ToList().ForEach(wallet => walletsDto.Add(_mapper.Map<WalletDto>(wallet)));
            return Ok(walletsDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet([FromBody]WalletForCreateDto walletForListDtos)
        {
            var user = await HttpContext.GetUserAsync(_userManager);
            var wallet = WalletFactory.CreateEmptyWallet(walletForListDtos.CurrencyCode);

            _mapper.Map(walletForListDtos, wallet);
            wallet.OwnerId = user.Id;

            await _repository.Add(wallet);
            await _repository.SaveChangesAsync();

            var walletDto = _mapper.Map<WalletDto>(wallet);

            return CreatedAtAction(nameof(GetWallet), new { id = walletDto.Id }, walletDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateWallet([FromBody]WalletForUpdateDto walletForUpdateDto)
        {
            var wallet = await _repository.Get(walletForUpdateDto.Id);
            if(wallet == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, wallet, "Permission");
            if (!AR.Succeeded)
            {
                return Forbid();
            }

            _mapper.Map(walletForUpdateDto, wallet);
            _repository.Update(wallet);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWallet(int id)
        {
            var wallet = await _repository.Get(id);
            if (wallet == null)
            {
                return NotFound();
            }

            var AR = await _authorizationService.AuthorizeAsync(HttpContext.User, wallet, "Permission");
            if (!AR.Succeeded)
            {
                return Forbid();
            }

            _repository.Remove(wallet);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("available")]
        public IActionResult GetAvailableWallets()
        {
            var userId = HttpContext.GetUserIdFromToken();
            var currencies = _repository.GetAvailableWallets(int.Parse(userId));

            return Ok(currencies);
        }

        [HttpGet("AmountCurrencies")]
        public IActionResult GetAllCurrenciesAmount()
        {
            var amount = _repository.GetAllCurrenciesAmount();

            return Ok(amount);
        }
    }
}
