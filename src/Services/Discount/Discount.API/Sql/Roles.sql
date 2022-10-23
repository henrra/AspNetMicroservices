-- Création rôle
do $$
declare
  role_name varchar(10) := 'microasp';
  role_passwd varchar(20) := 'microasp';
begin 
	if not exists (select from pg_catalog.pg_roles where rolname = role_name) then
		execute format(
	      'create role %I createdb connection limit 100 login  password %L',
	      role_name,
	      role_passwd
	    );    	
	end if;
end $$;
