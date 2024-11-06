CREATE OR REPLACE FUNCTION get_user_by_name(_name VARCHAR, _surname VARCHAR)
    RETURNS SETOF users AS
$$
DECLARE
    user_record users%ROWTYPE;
BEGIN
    FOR user_record IN
        SELECT * FROM Users u WHERE u.name = _name and u.surname = _surname
        LOOP
            RETURN NEXT user_record;
        END LOOP;
END;
$$ LANGUAGE plpgsql;
