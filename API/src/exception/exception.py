class AnalysisException(Exception):
    code = 500
    message = 'internal-error'
    description = 'Internal server error'
    pass

class EmptyDataException(AnalysisException):
    code = 400
    message = 'no-data'
    description = 'Empty data provided.'
    pass

class InvalidFileException(AnalysisException):
    code = 400
    message = 'invalid-file'
    description = 'An invalid file is provided. Cannot convert.'
    pass