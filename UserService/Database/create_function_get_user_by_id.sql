CREATE OR REPLACE FUNCTION get_user_by_id(_id INT)
    RETURNS SETOF users AS
$$
DECLARE
    user_record users%ROWTYPE;
BEGIN
    FOR user_record IN
        SELECT * FROM Users u WHERE u.id = _id
        LOOP
            RETURN NEXT user_record;
        END LOOP;
END;
$$ LANGUAGE plpgsql;