CREATE OR REPLACE FUNCTION update_user(_id INT, _name VARCHAR, _surname VARCHAR, _age INT)
RETURNS BOOLEAN AS $$
BEGIN
    UPDATE users
    SET name = _name, surname = _surname, age = _age
    WHERE id = _id;

    IF FOUND THEN
        RETURN TRUE;
    ELSE
        RETURN FALSE;
    END IF;
END;
$$ LANGUAGE plpgsql;
