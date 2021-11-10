using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using ViteReact.Shared.DTO;
using ViteReact.Shared.Repository;

namespace ViteReact.Shared.Controller
{
    public abstract class AppController<TEntity, TReadDTO> : ControllerBase
    {
        protected readonly IRepository<TEntity, TReadDTO> _repository;

        protected AppController(IRepository<TEntity, TReadDTO> repository)
        {
            _repository = repository;
        }

        protected ActionResult<ListReadDTO<TReadDTO>> GetEntities(PaginateDTO paginate)
        {
            var entities = _repository
                .Get(out int total)
                .Skip(paginate.Skip)
                .Limit(paginate.Limit)
                .ToList();

            return ReadableResult(entities, total);
        }

        protected ActionResult<TReadDTO> FindEntity(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity entity = _repository.Find(predicate);

            if (entity is null) return NotFound();

            return ReadableResult(entity);
        }

        protected ActionResult<ListReadDTO<TReadDTO>> FindEntities(PaginateDTO paginate, Expression<Func<TEntity, bool>> predicate)
        {
            var entities = _repository
            .Find(predicate, out int total)
            .Skip(paginate.Skip)
            .Limit(paginate.Limit)
            .ToList();

            return ReadableResult(entities, total);
        }

        protected ActionResult RemoveEntity(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                TEntity entity = _repository.Find(predicate);

                if (entity is null) return NotFound();

                return ApplyRemove(entity);
            }
            catch (System.Exception)
            {
                return UnprocessableEntity("Não foi possivel completar esta operação");
            }
        }

        protected ActionResult<TReadDTO> CreateEntity(TEntity entity)
        {
            _repository.Add(entity);

            if(!_repository.SaveChanges())
            {
                return UnprocessableEntity($"Falha ao criar novo registo do tipo {typeof(TEntity).Name}");
            }

            return ReadableResult(entity);
        }

        protected ActionResult<TReadDTO> UpdateEntity<TUpdateDTO>(TUpdateDTO data, Expression<Func<TEntity, bool>> predicate, Action<TEntity> preExecute = null)
        {
            TEntity entity = _repository.Find(predicate);

            if (entity is null) return NotFound();

            if(preExecute != null) preExecute(entity);

            _repository.Update(entity, data);

            _repository.SaveChanges();

            return ReadableResult(entity);
        }

        protected ActionResult ApplyRemove(TEntity entity)
        {
            _repository.Remove(entity);
            _repository.SaveChanges();

            return Ok();
        }

        protected ActionResult<TReadDTO> ReadableResult(TEntity source)
        {
            var result = _repository.ParseToRead(source);

            return Ok(result);
        }

        protected ActionResult<ListReadDTO<TReadDTO>> ReadableResult(IEnumerable<TEntity> source, int total)
        {
            var data = source.Select(_repository.ParseToRead);

            return Ok(new ListReadDTO<TReadDTO>
            {
                Data = data,
                Total = total,
            });
        }
    }
}