import React from 'react';
import { Col, Row } from 'antd';
import useWindowSize from '../../utils/useWindowSize';

export const SideBySide = ({ left, right, full, gutter = 16 }) => {
  const size = useWindowSize();

  return (
    size.width > 600 ? (
      <Row gutter={gutter}>
        {full ? (
          <Col span={24}>{full}</Col>
        ) : (
          <>
            <Col span={12}>{left}</Col>
            <Col span={12}>{right}</Col>
          </>
        )}
      </Row>
    ) : (
      <>
        <Row gutter={gutter}>
          <Col span={24}>{left}</Col>
        </Row>
        <Row gutter={gutter}>
          <Col span={24}>{right}</Col>
        </Row>
      </>
    )
  );
};
