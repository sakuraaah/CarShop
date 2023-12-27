import styled from 'styled-components';

export const StyledPage = styled.div`
  width: 100%;
  padding: 30px;
  border-radius: 6px;
  background-color: #f9f9f9;
  margin: 20px 0;
`;

export const SiderLogoWrapper = styled.div`
  width: 100%;
  padding: 15px;
`;

export const SiderLogoLink = styled.a`
  width: 100%;
  display: block;
  padding: 10px 0;
  background: #f5f5f5;
  border-radius: 8px;
  font-size: 18px;
  font-weight: 500;
  white-space: nowrap !important;
  overflow: hidden;
`;

export const SiderInfoWrapper = styled.div`
  width: 100%;
  padding: 25px;

  .ant-avatar {
    display: block;
    margin: 20px auto;
  }

  .label-form-item {
    margin-bottom: 12px;
  }
`;

export const StyledWrapper = styled.div`
  width: 100%;
  padding: 30px;
  border-radius: 6px;
  background-color: #fff;
  margin-bottom: 20px;

  .ant-row {

    &:not(:last-child) {
      padding-bottom: 15px;
    }
  }

  .ant-form-item:last-child {
    margin-bottom: 0;
  }
`;

export const FilterWrapper = styled.div`
  width: 100%;
  display: grid;
  grid-template-columns: 1fr 1fr 1fr 1fr;
  column-gap: 32px;

  .ant-picker {
    width: 100%;
  }
`;

export const FormHeader = styled.div`
  margin-bottom: 20px;
  display: flex;
  flex-direction: column;
  gap: 16px;

  .styled-label {
    font-size: 21px !important;
  }

  .ant-btn {
    width: 100%;
    max-width: 200px;
  }
`;

export const BorderBottom = styled.div`
  border-bottom: 1px solid #ddd;
  padding-bottom: 26px;
  margin-bottom: 26px;
`;

export const ButtonList = styled.div`
  display: flex;
  flex-wrap: wrap;
  justify-content: end;
  align-items: center;
  gap: 16px;

  &.left {
    justify-content: start;
  }

  &.small-gap {
    gap: 8px;
  }

  .ant-form-item {
    width: 100%;
    max-width: 400px;
    margin-right: auto;
  }
`;

export const Color = styled.p`
  &.green {
    color: #52c41a;
  }

  &.red {
    color: #ff4d4f;
  }
`;