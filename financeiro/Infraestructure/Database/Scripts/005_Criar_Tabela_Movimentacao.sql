CREATE TABLE movimentacao (
    id BIGSERIAL PRIMARY KEY,
    usuario_id BIGINT NOT NULL,
    categoria_id BIGINT NOT NULL,
    orcamento_id BIGINT NOT NULL,
    valor NUMERIC(18,2) NOT NULL,
    data_geracao TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
    data_movimentacao TIMESTAMP NOT NULL,
    descricao VARCHAR(255),
    tag VARCHAR(100)

    CONSTRAINT ck_movimentacao_valor CHECK (valor > 0),

    CONSTRAINT fk_mov_usuario 
        FOREIGN KEY (usuario_id) REFERENCES usuario(id),

    CONSTRAINT fk_mov_categoria 
        FOREIGN KEY (categoria_id) REFERENCES categoria(id),

    CONSTRAINT fk_mov_orcamento
        FOREIGN KEY (orcamento_id) REFERENCES orcamento(id)
);
CREATE INDEX idx_mov_usuario_data 
ON movimentacao(usuario_id, data_movimentacao);

CREATE INDEX idx_mov_orcamento
ON movimentacao(orcamento_id);
