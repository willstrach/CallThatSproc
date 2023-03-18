declare @DatabaseName nvarchar(max);
set @DatabaseName = 'CallThatSproc_EntityFrameworkTesting';

declare @connectionKillSql as nvarchar(max);
set @connectionKillSql = '';

select @connectionKillSql = @connectionKillSql + 'kill ' + convert(varchar, SPId) + ';'
from SysProcesses
where DBId = db_id(@DatabaseName) and SPId <> @@SPId;

declare @dropDatabaseSql as nvarchar(max);
set @dropDatabaseSql = 'drop database if exists ' + @DatabaseName;

exec(@connectionKillSql);
exec(@dropDatabaseSql);