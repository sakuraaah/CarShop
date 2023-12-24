import styled from 'styled-components';

export const StyledPage = styled.div`
  width: 100%;
  padding: 30px;
  border-radius: 6px;
  background-color: #f5f7f7;
  margin-bottom: 20px;
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
  gap: 16px;
`;