create proc sp_RegistrarUsuario(
  @Nombres varchar(100),
  @Apellidos varchar(100),
  @Correo varchar(100),
  @Clave varchar(100),
  @Activo bit,
  @Mensaje varchar(500) output,
  @Resultado int output
)
as
begin
  SET @Resultado = 0
  IF NOT EXISTS (SELECT * FROM USUARIO WHERE Correo = @Correo)
  begin
    insert into USUARIO(Nombres, Apellidos, Correo, Clave, Activo)
    values(@Nombres, @Apellidos, @Correo, @Clave, @Activo)

    SET @Resultado = scope_identity()
  end
  else
    set @Mensaje = 'El correo del usuario ya existe'
end


create proc sp_EditarUsuario(
  @IdUsuario int,
  @Nombres varchar(100),
  @Apellidos varchar(100),
  @Correo varchar(100),
  @Activo bit,
  @Mensaje varchar(500) output,
  @Resultado bit output
)
as
begin
  SET @Resultado = 0
  IF NOT EXISTS (SELECT * FROM USUARIO WHERE Correo = @Correo and IdUsuario != @IdUsuario)
  begin
    update top (1) USUARIO set
      Nombres = @Nombres,
      Apellidos = @Apellidos,
      Correo = @Correo,
      Activo = @Activo
    where IdUsuario = @IdUsuario

    SET @Resultado = 1
  end
  else
    set @Mensaje = 'El correo del usuario ya existe'
end


create proc sp_RegistrarCategoria(
    @Descripcion varchar(100),
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
) 
as 
begin
    -- Initialize the output result
    SET @Resultado = 0 

    -- Check if a category with the same description already exists
    IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion) 
    begin
        -- Insert the new category if it doesn't exist
        insert into CATEGORIA(Descripcion, Activo) 
        values (@Descripcion, @Activo)

        -- Set the output result to the new category ID
        SET @Resultado = scope_identity() 
    end
    else 
        -- Set the output message if the category already exists
        set @Mensaje = 'La categoria ya existe'
end


create proc sp_EditarCategoria(
    @IdCategoria int,
    @Descripcion varchar(100),
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado bit output
) 
as 
begin
    -- Initialize the output result
    SET @Resultado = 0 

    -- Check if a category with the same description exists (excluding the current one)
    IF NOT EXISTS (SELECT * FROM CATEGORIA WHERE Descripcion = @Descripcion and IdCategoria != @IdCategoria) 
    begin
        -- Update the category if no duplicate is found
        update top (1) CATEGORIA 
        set Descripcion = @Descripcion,
            Activo = @Activo
        where IdCategoria = @IdCategoria

        -- Set the output result to 1 (indicating success)
        SET @Resultado = 1 
    end
    else 
        -- Set the output message if a duplicate category exists
        set @Mensaje = 'La categoria ya existe'
end


create proc sp_EliminarCategoria(
    @IdCategoria int,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin
    -- Initialize the output result to 0 (failure)
    SET @Resultado = 0 

    -- Check if the category is not related to any product
    IF NOT EXISTS (
        select * 
        from PRODUCTO p 
        inner join CATEGORIA c on c.IdCategoria = p.IdCategoria 
        where p.IdCategoria = @IdCategoria
    )
    begin
        -- Delete the category if no related product is found
        delete top (1) from CATEGORIA where IdCategoria = @IdCategoria
        
        -- Set the output result to 1 (indicating success)
        SET @Resultado = 1 
    end
    else 
        -- Set the output message if the category is related to a product
        set @Mensaje = 'La categoria se encuentra relacionada a un producto'
end


create proc sp_RegistrarMarca(
    @Descripcion varchar(100),
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
)
as
begin
    -- Initialize the output result to 0 (indicating failure)
    SET @Resultado = 0 

    -- Check if a record with the same description already exists in the MARCA table
    IF NOT EXISTS (SELECT * FROM MARCA WHERE Descripcion = @Descripcion)
    begin
        -- Insert a new record into the MARCA table
        insert into MARCA(Descripcion, Activo) 
        values (@Descripcion, @Activo)

        -- Set the output result to the new ID of the inserted record
        SET @Resultado = scope_identity() 
    end
    else 
        -- Set the output message indicating that the brand already exists
        set @Mensaje = 'La marca ya existe'
end

create proc sp_EditarMarca(
    @IdMarca int,
    @Descripcion varchar(100),
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin
    -- Initialize the output result to 0 (indicating failure)
    SET @Resultado = 0 

    -- Check if there is already a record with the same description and a different IdMarca
    IF NOT EXISTS (SELECT * FROM MARCA WHERE Descripcion = @Descripcion and IdMarca != @IdMarca)
    begin
        -- Update the record in the MARCA table
        update top (1) MARCA set 
            Descripcion = @Descripcion, 
            Activo = @Activo
        where IdMarca = @IdMarca

        -- Set the output result to 1 (indicating success)
        SET @Resultado = 1 
    end
    else 
        -- Set the output message indicating that the brand already exists
        set @Mensaje = 'La marca ya existe'
end

create proc sp_EliminarMarca(
    @IdMarca int,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin
    -- Initialize the output result to 0 (indicating failure)
    SET @Resultado = 0 

    -- Check if the brand is not associated with any products
    IF NOT EXISTS (
        SELECT * 
        FROM PRODUCTO p 
        INNER JOIN MARCA m ON m.IdMarca = p.IdMarca 
        WHERE p.IdMarca = @IdMarca
    )
    begin
        -- Delete the brand from the MARCA table
        delete top (1) from MARCA where IdMarca = @IdMarca

        -- Set the output result to 1 (indicating success)
        SET @Resultado = 1 
    end
    else 
        -- Set the output message indicating that the brand is associated with a product
        set @Mensaje = 'La marca se encuentra relacionada a un producto'
end

create proc sp_RegistrarProducto(
    @Nombre varchar(100),
    @Descripcion varchar(100),
    @IdMarca varchar(100),
    @IdCategoria varchar(100),
    @Precio decimal(10,2),
    @Stock int,
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado int output
)
as
begin
    -- Initialize the output result to 0 (indicating failure)
    SET @Resultado = 0 

    -- Check if a product with the same name already exists
    IF NOT EXISTS (
        SELECT * 
        FROM PRODUCTO 
        WHERE Nombre = @Nombre
    )
    begin
        -- Insert the product into the PRODUCTO table
        insert into PRODUCTO(Nombre, Descripcion, IdMarca, IdCategoria, Precio, Stock, Activo) 
        values(@Nombre, @Descripcion, @IdMarca, @IdCategoria, @Precio, @Stock, @Activo)

        -- Set the output result to the new product ID
        SET @Resultado = scope_identity()
    end
    else 
        -- Set the output message indicating that the product already exists
        set @Mensaje = 'El producto ya existe'
end

create proc sp_EditarProducto(
    @IdProducto int,
    @Nombre varchar(100),
    @Descripcion varchar(100),
    @IdMarca varchar(100),
    @IdCategoria varchar(100),
    @Precio decimal(10,2),
    @Stock int,
    @Activo bit,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin
    -- Initialize the output result to 0 (indicating failure)
    SET @Resultado = 0 

    -- Check if a product with the same name exists, excluding the current product
    IF NOT EXISTS (
        SELECT * 
        FROM PRODUCTO 
        WHERE Nombre = @Nombre AND IdProducto != @IdProducto
    )
    begin
        -- Update the product in the PRODUCTO table
        update PRODUCTO 
        set 
            Nombre = @Nombre,
            Descripcion = @Descripcion,
            IdMarca = @IdMarca,
            IdCategoria = @IdCategoria,
            Precio = @Precio,
            Stock = @Stock,
            Activo = @Activo
        where IdProducto = @IdProducto

        -- Set the output result to 1 (indicating success)
        SET @Resultado = 1
    end
    else 
        -- Set the output message indicating that the product already exists
        set @Mensaje = 'El producto ya existe'
end

create proc sp_EliminarProducto(
    @IdProducto int,
    @Mensaje varchar(500) output,
    @Resultado bit output
)
as
begin
    -- Initialize the output result to 0 (indicating failure)
    SET @Resultado = 0 

    -- Check if the product is not linked to any sales details
    IF NOT EXISTS (
        SELECT * 
        FROM DETALLE_VENTA dv
        INNER JOIN PRODUCTO p ON p.IdProducto = dv.IdProducto
        WHERE p.IdProducto = @IdProducto
    )
    begin
        -- Delete the product from the PRODUCTO table
        delete top (1) from PRODUCTO where IdProducto = @IdProducto

        -- Set the output result to 1 (indicating success)
        SET @Resultado = 1
    end
    else 
        -- Set the output message indicating that the product is linked to a sale
        set @Mensaje = 'El producto se encuentra relacionado a una venta'
end

CREATE PROC sp_RegistrarCliente(
  @Nombres VARCHAR(100),
  @Apellidos VARCHAR(100),
  @Correo VARCHAR(100),
  @Clave VARCHAR(150),
  @Reestablecer BIT,
  @Mensaje VARCHAR(500) OUTPUT,
  @Resultado INT OUTPUT
)
AS
BEGIN
  SET @Resultado = 0;
  IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Correo = @Correo)
  BEGIN
    INSERT INTO CLIENTE(Nombres, Apellidos, Correo, Clave, Reestablecer, FechaRegistro)
    VALUES(@Nombres, @Apellidos, @Correo, @Clave, @Reestablecer, GETDATE());

    SET @Resultado = SCOPE_IDENTITY();
  END
  ELSE
    SET @Mensaje = 'El correo del cliente ya existe';
END;

CREATE PROC sp_EditarCliente(
  @IdCliente INT,
  @Nombres VARCHAR(100),
  @Apellidos VARCHAR(100),
  @Correo VARCHAR(100),
  @Reestablecer BIT,
  @Mensaje VARCHAR(500) OUTPUT,
  @Resultado BIT OUTPUT
)
AS
BEGIN
  SET @Resultado = 0;
  IF NOT EXISTS (SELECT * FROM CLIENTE WHERE Correo = @Correo AND IdCliente != @IdCliente)
  BEGIN
    UPDATE CLIENTE
    SET Nombres = @Nombres,
        Apellidos = @Apellidos,
        Correo = @Correo,
        Reestablecer = @Reestablecer
    WHERE IdCliente = @IdCliente;

    SET @Resultado = 1;
  END
  ELSE
    SET @Mensaje = 'El correo del cliente ya existe';
END;
