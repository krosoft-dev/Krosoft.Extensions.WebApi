

//using System.ComponentModel.DataAnnotations;
//using IzRoadbook.Api.Handlers.Queries.Roadbooks;
//using IzRoadbook.Api.Models.Commands.Roadbooks;
//using IzRoadbook.Api.Models.Dto;
//using IzRoadbook.Api.Models.Queries.Roadbooks;
//using IzRoadbook.Extensions.Constantes;
//using IzRoadbook.Extensions.Controllers;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Positive.Extensions.AspNetCore.Extensions;
//using Positive.Extensions.AspNetCore.Models;
//using Krosoft.Extensions.Core.Models.MyProject.Common;
//using Microsoft.AspNetCore.Mvc;

//namespace Krosoft.Extensions.WebApi.Extensions;

//public static class QueryResultExtensions
//{
//    public static IActionResult ToActionResult(this QueryResult result)
//    {
//        if(result.Success)
//        {
//            return new OkResult();
//        }

//        if(result.Errors != null)
//        {
//            // ToModelStateDictionary needs to be implemented by you ;)
//            var errors = result.Errors.ToModelStateDictionary();
//            return BadRequestResult(errors);
//        }

//        return BadRequestResult();
//    }

//    public static IActionResult ToActionResult<T>(this QueryResult<T> result)
//    {
//        if(result.Success)
//        {
//            if(result.Result == null)
//            {
//                return new NotFoundResult();
//            }

//            return new OkObjectResult(result.Result);
//        }

//        if(result.Errors != null)
//        {
//            // ToModelStateDictionary needs to be implemented by you ;)
//            var errors = result.Errors.ToModelStateDictionary();
//            return BadRequestResult(errors);
//        }

//        return BadRequestResult();
//    }
//}







//namespace IzRoadbook.Api.Controllers;

///// <summary>
///// Initialise a new instance of <see cref="RoadbooksController" />.
///// </summary>
//[ApiController]
//public class RoadbooksController : ApiControllerBase
//{
//	/// <summary>
//	/// Sauvegarde le fichier en vu du traitement
//	/// </summary>
//	[AllowAnonymous]
//	[HttpPost("Upload")]
//	[DisableRequestSizeLimit]
//	public async Task<RoadbookUploadResultDto> UploadAsync(CancellationToken cancellationToken)
//	{
//		var files = (await Request.ReadFormAsync(cancellationToken)).Files;
//		return await Mediator.Send(new RoadbookUploadCommand(files), cancellationToken);
//	}

//	[AllowAnonymous]
//	[HttpPost("Preview")]
//	public Task<IEnumerable<string>> Preview([FromBody] ModelDto model,
//		CancellationToken cancellationToken)
//		=> Mediator.Send(new RoadbookPreviewQuery(model), cancellationToken);

//	[AllowAnonymous]
//	[HttpPost("Zip")]
//	[ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK, HttpContentType.ApplicationZip)]
//	[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
//	[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
//	public Task<FileStreamResult> Zip([FromBody] ModelDto model,
//		CancellationToken cancellationToken)
//		=> Mediator.Send(new RoadbookZipQuery(model), cancellationToken).ToFileStreamResult();

//	/// <summary>
//	/// Get a roadbook using an id
//	/// </summary>
//	/// <param name="id">the roadbook id</param>
//	/// <param name="cancellationToken"></param>
//	/// <returns>return a roadbookDto</returns>
//	[HttpGet("{id:Guid}")]
//	[ProducesResponseType(typeof(RoadbookDto), StatusCodes.Status200OK)]
//	[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
//	[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
//	public async Task<RoadbookDto> GetAsync(Guid id, CancellationToken cancellationToken)
//		=> await Mediator.Send(new RoadbookQuery(id), cancellationToken);

//	/// <summary>
//	/// Get roadbooks by authenticated user
//	/// </summary>
//	/// <param name="cancellationToken"></param>
//	/// <returns>List of roadbooks</returns>
//	[Authorize]
//	[HttpGet]
//	[ProducesResponseType(typeof(IEnumerable<RoadbookDto>), StatusCodes.Status200OK)]
//	[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
//	[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
//	public async Task<IEnumerable<RoadbookDto>> GetRoadbooksByAuthUserAsync(CancellationToken cancellationToken)
//		=> await Mediator.Send(new RoadbooksQuery(), cancellationToken);

//	/// <summary>
//	/// Create a Roadbook
//	/// </summary>
//	/// <param name="cancellationToken"></param>
//	/// <returns></returns>
//	[HttpPost]
//	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
//	[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
//	public async Task<Guid> CreateAsync([FromBody][Required] CreateRoadbookCommand command, CancellationToken cancellationToken)
//	{
//		var files = (await Request.ReadFormAsync(cancellationToken)).Files;
//		command.Files = files;
//		var result = await Mediator.Send(command, cancellationToken);
//		Response.StatusCode = StatusCodes.Status201Created;
//		return result;
//	}

//	/// <summary>
//	/// Delete a Roadbook using an id
//	/// </summary>
//	/// <param name="command">command of the roadbook item that will be delete</param>
//	/// <param name="cancellationToken"></param>
//	/// <returns></returns>
//	[HttpDelete]
//	[ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
//	[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status401Unauthorized)]
//	[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status404NotFound)]
//	[ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
//	public async Task DeleteAsync([FromBody][Required] DeleteRoadbookCommand command,
//		CancellationToken cancellationToken)
//		=> await Mediator.Send(command, cancellationToken);
//}