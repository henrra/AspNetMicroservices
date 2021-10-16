-- Coupon
create table coupon(
   id serial primary key not null,
   product_name varchar(24) not null,
   description text,
   amount int
);

-- Tests
insert into coupon(product_name, description, amount) values('IPhone 13', 'IPhone Discount', 1680);
insert into coupon(product_name, description, amount) values('Huawei P40', 'Huawei Discount', 1200);