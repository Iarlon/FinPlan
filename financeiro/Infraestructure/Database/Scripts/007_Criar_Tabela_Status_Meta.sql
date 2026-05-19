CREATE TABLE status_meta (
    id BIGSERIAL PRIMARY KEY,
    descricao VARCHAR(255) NOT NULL
);

INSERT INTO status_meta (id, descricao)
VALUES
(1,'Concluido'),
(2,'AcimaDoPlanejado'),
(3,'AbaixoDoPlanejado'),
(4,'Cancelado'),
(5, 'NaMeta')